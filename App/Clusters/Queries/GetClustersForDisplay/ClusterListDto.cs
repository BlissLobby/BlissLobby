namespace App.Clusters.Queries.GetClustersForDisplay;

public class ClusterListDto
{
    public required string Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public List<string> AdminNames { get; set; } = [];
}
