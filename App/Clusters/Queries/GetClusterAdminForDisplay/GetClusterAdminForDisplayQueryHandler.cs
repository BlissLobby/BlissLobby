using App.Common.Interfaces;

namespace App.Clusters.Queries.GetClusterAdminForDisplay;

public class GetClusterAdminForDisplayQueryHandler(IApplicationDbContext dbContext)
    : IRequestHandler<GetClusterAdminForDisplayQuery, ClusterAdminDisplayDto?>
{
    public async Task<ClusterAdminDisplayDto?> Handle(
        GetClusterAdminForDisplayQuery request,
        CancellationToken cancellationToken)
    {
        return await dbContext.ClusterAdmins
            .AsNoTracking()
            .Where(clusterAdmin => clusterAdmin.Id == request.Id)
            .Select(clusterAdmin => new ClusterAdminDisplayDto
            {
                Id = clusterAdmin.Id,
                ClusterId = clusterAdmin.ClusterId,
                ClusterName = clusterAdmin.Cluster.Name,
                AdminDisplayName = clusterAdmin.Admin.DisplayName
            })
            .FirstOrDefaultAsync(cancellationToken);
    }
}
