using App.Common.Security;
using Domain.Constants;

namespace App.Clusters.Commands.UpdateCluster;

[Authorize(Roles = Roles.Administrator)]
public class UpdateClusterCommand : IRequest<bool>
{
    public string Id { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
}
