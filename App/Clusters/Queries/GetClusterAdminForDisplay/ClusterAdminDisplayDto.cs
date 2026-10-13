namespace App.Clusters.Queries.GetClusterAdminForDisplay;

public class ClusterAdminDisplayDto
{
    public required string Id { get; set; }
    public required string ClusterId { get; set; }
    public required string ClusterName { get; set; }
    public required string AdminDisplayName { get; set; }
}
