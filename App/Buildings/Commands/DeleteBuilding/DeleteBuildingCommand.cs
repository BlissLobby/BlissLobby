using App.Common.Interfaces;
using App.Common.Security;
using Domain.Constants;

namespace App.Buildings.Commands.DeleteBuilding;

[Authorize(Roles = $"{Roles.Administrator},{Roles.ClusterAdmin}")]
public record DeleteBuildingCommand(string Id) : IRequest<bool>;

public class DeleteBuildingCommandHandler(IApplicationDbContext dbContext) : IRequestHandler<DeleteBuildingCommand, bool>
{
    public async Task<bool> Handle(DeleteBuildingCommand request, CancellationToken cancellationToken)
    {
        var building = await dbContext.Buildings.FindAsync([request.Id], cancellationToken);

        if (building == null)
            return false;

        dbContext.Buildings.Remove(building);
        await dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }
}
