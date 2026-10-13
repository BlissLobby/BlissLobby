namespace App.Clusters.Queries.GetClusterAdminUsers;

public class ClusterAdminUserDto
{
    public required string Id { get; set; }
    public required string UserName { get; set; }
    public required string DisplayName { get; set; }
}
