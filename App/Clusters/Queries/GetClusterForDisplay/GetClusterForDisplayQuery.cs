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


        ClusterDto? clusterDto = cluster != null ? new ClusterDto()
        {
            Id = cluster.Id,
            Name = cluster.Name,
            Description = cluster.Description,
            AdminNames = [.. dbContext.ClusterAdmins
                .AsNoTracking()
                .Where(ca => ca.ClusterId == cluster.Id)
                .Include(ca => ca.Admin)
                .Select(ca => ca.Admin.DisplayName)]
        } : null;

        return clusterDto;
    }
}
