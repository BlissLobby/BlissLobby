using App.Common.Interfaces;
using App.Common.Pagination;

namespace App.Buildings.Queries.GetBuildingsForDisplay;

public class GetBuildingsForDisplayQueryHandler(IApplicationDbContext dbContext) : IRequestHandler<GetBuildingsForDisplayQuery, PaginationResponse<BuildingListDto>>
{
    public async Task<PaginationResponse<BuildingListDto>> Handle(GetBuildingsForDisplayQuery buildingsQuery, CancellationToken cancellationToken)
    {
        var req = buildingsQuery.Query;
        IQueryable<Domain.Entities.Building> buildings = dbContext.Buildings.Include(b => b.Cluster).AsNoTracking();

        // filter the buildings as per each of req.Filters
        buildings = buildings.ApplyFilters(req.Filters);

        // sort the buildings as per req.SortBy and req.SortDescending
        buildings = buildings.ApplySort($"{req.SortBy} {(req.SortDesc ? "desc" : "asc")}");

        // get paginated results
        buildings = buildings.ApplyPagination(req.Page, req.PageSize);

        List<BuildingListDto> buildingDtos = [];

        foreach (var building in buildings)
        {
            var buildingDto = new BuildingListDto
            {
                Id = building.Id,
                Name = building.Name,
                Address = building.Address,
                City = building.City,
                State = building.State,
                ZipCode = building.ZipCode,
                Country = building.Country,
                ClusterName = building.Cluster?.Name ?? "N/A",
                Description = building.Description
            };

            buildingDtos.Add(buildingDto);
        }

        int totalCount = -1;
        if (req.RequestNewCount)
        {
            totalCount = await dbContext.Buildings.CountAsync(cancellationToken);
        }
        return new PaginationResponse<BuildingListDto>(buildingDtos, totalCount);
    }
}
