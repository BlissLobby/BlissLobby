using App.Common.Interfaces;
using App.Common.Security;

namespace App.Buildings.Queries.GetBuildingForDisplay;

[Authorize]
public record GetBuildingForDisplayQuery(string Id) : IRequest<BuildingDto?>;

public class GetBuildingForDisplayQueryHandler(IApplicationDbContext dbContext) : IRequestHandler<GetBuildingForDisplayQuery, BuildingDto?>
{
    public async Task<BuildingDto?> Handle(GetBuildingForDisplayQuery buildingQuery, CancellationToken cancellationToken)
    {
        var building = await dbContext.Buildings
            .AsNoTracking()
            .FirstOrDefaultAsync(b => b.Id == buildingQuery.Id, cancellationToken);

        BuildingDto? buildingDto = building != null ? new BuildingDto()
        {
            Id = building.Id,
            ClusterId = building.ClusterId,
            Name = building.Name,
            Address = building.Address,
            City = building.City,
            State = building.State,
            ZipCode = building.ZipCode,
            Country = building.Country,
            Description = building.Description
        } : null;

        return buildingDto;
    }
}
