using App.Common.Pagination;
using App.Common.Security;
using Domain.Constants;

namespace App.Buildings.Queries.GetBuildingsForDisplay;

[Authorize(Roles = $"{Roles.Administrator},{Roles.ClusterAdmin}")]
public record GetBuildingsForDisplayQuery(PaginationQuery Query) : IRequest<PaginationResponse<BuildingListDto>>;
