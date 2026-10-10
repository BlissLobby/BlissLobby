using App.Common.Interfaces;

namespace App.Clusters.Commands.UpdateCluster;

public class UpdateClusterCommandHandler(IApplicationDbContext dbContext) : IRequestHandler<UpdateClusterCommand, bool>
{
    public async Task<bool> Handle(UpdateClusterCommand request, CancellationToken cancellationToken)
    {
        var cluster = await dbContext.Clusters.FindAsync([request.Id], cancellationToken);

        if (cluster == null)
            return false;

        cluster.Name = request.Name;
        cluster.Description = request.Description;

        dbContext.Clusters.Update(cluster);
        await dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }
}
