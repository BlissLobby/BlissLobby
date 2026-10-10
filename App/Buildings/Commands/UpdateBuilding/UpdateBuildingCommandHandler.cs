using App.Common.Interfaces;

namespace App.Buildings.Commands.UpdateBuilding;

public class UpdateBuildingCommandHandler(IApplicationDbContext dbContext) : IRequestHandler<UpdateBuildingCommand, bool>
{
    public async Task<bool> Handle(UpdateBuildingCommand request, CancellationToken cancellationToken)
    {
        var building = await dbContext.Buildings.FindAsync([request.Id], cancellationToken);

        if (building == null)
            return false;

        building.ClusterId = request.ClusterId;
        building.Name = request.Name;
        building.Address = request.Address;
        building.City = request.City;
        building.State = request.State;
        building.ZipCode = request.ZipCode;
        building.Country = request.Country;
        building.Description = request.Description;

        dbContext.Buildings.Update(building);
        await dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }
}
