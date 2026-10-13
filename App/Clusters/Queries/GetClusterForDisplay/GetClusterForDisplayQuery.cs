using App.Common.Interfaces;
using App.Common.Security;

namespace App.Clusters.Queries.GetClusterForDisplay;

[Authorize]
public record GetClusterForDisplayQuery(string Id) : IRequest<ClusterDto?>;

public class GetClusterForDisplayQueryHandler(IApplicationDbContext dbContext) : IRequestHandler<GetClusterForDisplayQuery, ClusterDto?>
{
    public async Task<ClusterDto?> Handle(GetClusterForDisplayQuery clusterQuery, CancellationToken cancellationToken)
    {
        var cluster = await dbContext.Clusters
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == clusterQuery.Id, cancellationToken);


        if (cluster == null)
            return null;

        var clusterAdmins = await dbContext.ClusterAdmins
            .AsNoTracking()
            .Where(ca => ca.ClusterId == cluster.Id)
            .Include(ca => ca.Admin)
            .ToListAsync(cancellationToken);

        ClusterDto clusterDto = new()
        {
            Id = cluster.Id,
            Name = cluster.Name,
            Description = cluster.Description,
            AdminNames = [.. clusterAdmins.Select(ca => ca.Admin.DisplayName)],
            ClusterAdmins = [.. clusterAdmins.Select(ca => new ClusterAdminDto
            {
                Id = ca.Id,
                AdminId = ca.AdminId,
                AdminDisplayName = ca.Admin.DisplayName
            })]
        };

        return clusterDto;
    }
}
