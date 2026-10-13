namespace App.Clusters.Queries.GetClusterForDisplay;

public class ClusterDto
{
    public required string Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public List<string> AdminNames { get; set; } = [];
    public List<ClusterAdminDto> ClusterAdmins { get; set; } = [];
}
