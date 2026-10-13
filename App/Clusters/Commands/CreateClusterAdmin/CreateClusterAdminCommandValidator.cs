namespace App.Clusters.Commands.CreateClusterAdmin;

public class CreateClusterAdminCommandValidator : AbstractValidator<CreateClusterAdminCommand>
{
    public CreateClusterAdminCommandValidator()
    {
        RuleFor(command => command.ClusterId).NotEmpty();
        RuleFor(command => command.AdminId).NotEmpty();
    }
}
