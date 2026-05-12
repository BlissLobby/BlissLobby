using Domain.Common;

namespace Domain.Entities;

public class Building : BaseAuditableEntity
{
    public string ClusterId { get; set; } = default!;
    public Cluster Cluster { get; set; } = null!;

    public string Name { get; set; } = default!;
    public string Address { get; set; } = default!;
    public string City { get; set; } = default!;
    public string State { get; set; } = default!;
    public string ZipCode { get; set; } = default!;
    public string Country { get; set; } = default!;
    public string? Description { get; set; }

}
