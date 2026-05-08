using App.Common.Interfaces;
using App.Common.Security;
using Domain.Entities;

namespace App.Buildings.Queries.GetBuildings;

[Authorize]
public record GetBuildingsQuery : IRequest<List<Building>>;

public class GetBuildingsQueryHandler(IApplicationDbContext dbContext) : IRequestHandler<GetBuildingsQuery, List<Building>>
{
    public async Task<List<Building>> Handle(GetBuildingsQuery req, CancellationToken cancellationToken)
    {
        List<Building> buildings = await dbContext.Buildings.AsNoTracking().ToListAsync(cancellationToken);
        return buildings;
    }
}