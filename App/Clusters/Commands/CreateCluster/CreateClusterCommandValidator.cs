namespace App.Clusters.Commands.CreateCluster;

public class CreateClusterCommandValidator : AbstractValidator<CreateClusterCommand>
{
    public CreateClusterCommandValidator()
    {
        RuleFor(v => v.Name)
            .NotEmpty().WithMessage("Cluster name is required")
            .MinimumLength(2).WithMessage("Cluster name must be at least 2 characters long")
            .MaximumLength(100).WithMessage("Cluster name must be at most 100 characters long");

        RuleFor(v => v.Description)
            .MaximumLength(500).WithMessage("Description must be at most 500 characters long");
    }
}
