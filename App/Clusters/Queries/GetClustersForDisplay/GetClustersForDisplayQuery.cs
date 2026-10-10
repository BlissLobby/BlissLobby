using App.Common.Pagination;
using App.Common.Security;

namespace App.Clusters.Queries.GetClustersForDisplay;

[Authorize]
public record GetClustersForDisplayQuery(PaginationQuery Query) : IRequest<PaginationResponse<ClusterListDto>>;
