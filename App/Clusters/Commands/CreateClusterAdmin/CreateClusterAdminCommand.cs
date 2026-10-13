using App.Common.Security;
using Domain.Constants;

namespace App.Clusters.Commands.CreateClusterAdmin;

[Authorize(Roles = Roles.Administrator)]
public class CreateClusterAdminCommand : IRequest<bool>
{
    public string ClusterId { get; set; } = default!;
    public string AdminId { get; set; } = default!;
}
