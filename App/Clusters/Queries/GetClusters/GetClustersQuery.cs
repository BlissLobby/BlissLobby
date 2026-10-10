using App.Common.Interfaces;
using App.Common.Security;

namespace App.Clusters.Queries.GetClusters;

[Authorize]
public record GetClustersQuery : IRequest<List<Domain.Entities.Cluster>>;

public class GetClustersQueryHandler(IApplicationDbContext dbContext) : IRequestHandler<GetClustersQuery, List<Domain.Entities.Cluster>>
{
    public async Task<List<Domain.Entities.Cluster>> Handle(GetClustersQuery req, CancellationToken cancellationToken)
    {
        List<Domain.Entities.Cluster> clusters = await dbContext.Clusters.AsNoTracking().ToListAsync(cancellationToken);
        return clusters;
    }
}
