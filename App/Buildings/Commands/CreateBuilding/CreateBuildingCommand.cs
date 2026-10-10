using App.Common.Security;
using Domain.Constants;

namespace App.Buildings.Commands.CreateBuilding;

[Authorize(Roles = $"{Roles.Administrator},{Roles.ClusterAdmin}")]
public class CreateBuildingCommand : IRequest<string>
{
    public string ClusterId { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string Address { get; set; } = default!;
    public string City { get; set; } = default!;
    public string State { get; set; } = default!;
    public string ZipCode { get; set; } = default!;
    public string Country { get; set; } = default!;
    public string? Description { get; set; }
}
