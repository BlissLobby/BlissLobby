using App.Common.Security;
using Domain.Constants;

namespace App.Clusters.Commands.CreateCluster;

[Authorize(Roles = Roles.Administrator)]
public class CreateClusterCommand : IRequest<string>
{
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
}
