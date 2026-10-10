using App.Common.Interfaces;

namespace App.Buildings.Commands.CreateBuilding;

public class CreateBuildingCommandHandler(IApplicationDbContext dbContext) : IRequestHandler<CreateBuildingCommand, string>
{
    public async Task<string> Handle(CreateBuildingCommand request, CancellationToken cancellationToken)
    {
        var building = new Domain.Entities.Building
        {
            ClusterId = request.ClusterId,
            Name = request.Name,
            Address = request.Address,
            City = request.City,
            State = request.State,
            ZipCode = request.ZipCode,
            Country = request.Country,
            Description = request.Description
        };

        dbContext.Buildings.Add(building);
        await dbContext.SaveChangesAsync(cancellationToken);

        return building.Id;
    }
}
