using App.Common.Interfaces;
using App.Utils;

namespace App.Users.Commands.CreateUser;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    private readonly IApplicationDbContext _dbContext;

    public CreateUserCommandValidator(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;

        RuleFor(v => v.Username)
            .NotEmpty().WithMessage("Username is required")
            .MinimumLength(3).WithMessage("Username must be at least 3 characters long")
            .MaximumLength(20).WithMessage("Username must be at most 20 characters long");

        RuleFor(v => v.DisplayName)
            .NotEmpty().WithMessage("Display name is required")
            .MinimumLength(3).WithMessage("Display name must be at least 3 characters long")
            .MaximumLength(50).WithMessage("Display name must be at most 50 characters long");

        RuleFor(v => v.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format");

        RuleFor(v => v.PhoneNumber)
            .Must(FluentValidationUtils.BeAValidPhoneNumber).WithMessage("Invalid phone number format");

        RuleFor(v => v.UserRole)
            .Must(FluentValidationUtils.BeAValidRole).WithMessage("Invalid role provided");

        RuleFor(v => v.BuildingId)
            .MustAsync(BeAValidBuildingId).WithMessage("Building Id is not present in the database");

        RuleFor(v => v.Password)
                .NotEmpty().WithMessage("Password is required")
                .Equal(v => v.ConfirmPassword).WithMessage("Passwords do not match");

    }

    // custom rule to validate BuildingId
    private async Task<bool> BeAValidBuildingId(string? buildingId, CancellationToken cancellationToken)
    {
        if (buildingId == null) return true;

        return !string.IsNullOrWhiteSpace(buildingId) && 
                    await _dbContext.Buildings.AsNoTracking()
                            .AnyAsync(b => b.Id == buildingId, cancellationToken);
    }
}
