using App.Common.Interfaces;

namespace App.Buildings.Commands.CreateBuilding;

public class CreateBuildingCommandValidator : AbstractValidator<CreateBuildingCommand>
{
    private readonly IApplicationDbContext _dbContext;

    public CreateBuildingCommandValidator(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;

        RuleFor(v => v.ClusterId)
            .NotEmpty().WithMessage("Cluster is required")
            .MustAsync(BeAValidClusterId).WithMessage("Cluster does not exist in the database");

        RuleFor(v => v.Name)
            .NotEmpty().WithMessage("Building name is required")
            .MinimumLength(2).WithMessage("Building name must be at least 2 characters long")
            .MaximumLength(100).WithMessage("Building name must be at most 100 characters long");

        RuleFor(v => v)
            .MustAsync(BeUniqueBuildingName)
            .WithMessage("A building with this name already exists in the cluster");

        RuleFor(v => v.Address)
            .NotEmpty().WithMessage("Address is required")
            .MaximumLength(200).WithMessage("Address must be at most 200 characters long");

        RuleFor(v => v.City)
            .NotEmpty().WithMessage("City is required")
            .MaximumLength(50).WithMessage("City must be at most 50 characters long");

        RuleFor(v => v.State)
            .NotEmpty().WithMessage("State is required")
            .MaximumLength(50).WithMessage("State must be at most 50 characters long");

        RuleFor(v => v.ZipCode)
            .NotEmpty().WithMessage("Zip code is required")
            .MaximumLength(10).WithMessage("Zip code must be at most 10 characters long");

        RuleFor(v => v.Country)
            .NotEmpty().WithMessage("Country is required")
            .MaximumLength(50).WithMessage("Country must be at most 50 characters long");

        RuleFor(v => v.Description)
            .MaximumLength(500).WithMessage("Description must be at most 500 characters long");
    }

    private async Task<bool> BeAValidClusterId(string clusterId, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(clusterId)) return false;

        return await _dbContext.Clusters.AsNoTracking()
            .AnyAsync(c => c.Id == clusterId, cancellationToken);
    }

    private async Task<bool> BeUniqueBuildingName(CreateBuildingCommand command, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(command.Name)) return false;
        // validate if building name is unique within a cluster
        return !await _dbContext.Buildings.AsNoTracking()
            .AnyAsync(b => b.ClusterId == command.ClusterId && b.Name == command.Name, cancellationToken);
    }


}
