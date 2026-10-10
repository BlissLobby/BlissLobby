using App.Common.Interfaces;
using App.Common.Pagination;

namespace App.Clusters.Queries.GetClustersForDisplay;

public class GetClustersForDisplayQueryHandler(IApplicationDbContext dbContext) : IRequestHandler<GetClustersForDisplayQuery, PaginationResponse<ClusterListDto>>
{
    public async Task<PaginationResponse<ClusterListDto>> Handle(GetClustersForDisplayQuery clustersQuery, CancellationToken cancellationToken)
    {
        var req = clustersQuery.Query;
        IQueryable<Domain.Entities.Cluster> clusters = dbContext.Clusters.AsNoTracking();

        // filter the clusters as per each of req.Filters
        clusters = clusters.ApplyFilters(req.Filters);

        // sort the clusters as per req.SortBy and req.SortDescending
        clusters = clusters.ApplySort($"{req.SortBy} {(req.SortDesc ? "desc" : "asc")}");

        // get paginated results
        clusters = clusters.ApplyPagination(req.Page, req.PageSize);

        List<ClusterListDto> clusterDtos = [];

        foreach (var cluster in clusters)
        {
            // Get cluster admins separately
            var adminNames = await dbContext.ClusterAdmins
                .Where(ca => ca.ClusterId == cluster.Id)
                .Include(ca => ca.Admin)
                .Select(ca => ca.Admin.DisplayName)
                .ToListAsync(cancellationToken);

            var clusterDto = new ClusterListDto
            {
                Id = cluster.Id,
                Name = cluster.Name,
                Description = cluster.Description,
                AdminNames = adminNames
            };

            clusterDtos.Add(clusterDto);
        }

        int totalCount = -1;
        if (req.RequestNewCount)
        {
            totalCount = await dbContext.Clusters.CountAsync(cancellationToken);
        }
        return new PaginationResponse<ClusterListDto>(clusterDtos, totalCount);
    }
}

