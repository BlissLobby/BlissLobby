namespace App.Clusters.Commands.UpdateCluster;

public class UpdateClusterCommandValidator : AbstractValidator<UpdateClusterCommand>
{
    public UpdateClusterCommandValidator()
    {
        RuleFor(v => v.Id)
            .NotEmpty().WithMessage("Cluster ID is required");

        RuleFor(v => v.Name)
            .NotEmpty().WithMessage("Cluster name is required")
            .MinimumLength(2).WithMessage("Cluster name must be at least 2 characters long")
            .MaximumLength(100).WithMessage("Cluster name must be at most 100 characters long");

        RuleFor(v => v.Description)
            .MaximumLength(500).WithMessage("Description must be at most 500 characters long");
    }
}
