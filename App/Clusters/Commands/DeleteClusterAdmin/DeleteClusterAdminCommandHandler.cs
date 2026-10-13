using App.Common.Interfaces;

namespace App.Clusters.Commands.DeleteClusterAdmin;

public class DeleteClusterAdminCommandHandler(IApplicationDbContext dbContext) : IRequestHandler<DeleteClusterAdminCommand, bool>
{
    public async Task<bool> Handle(DeleteClusterAdminCommand request, CancellationToken cancellationToken)
    {
        var clusterAdmin = await dbContext.ClusterAdmins.Where(ca=>ca.Id == request.Id).FirstOrDefaultAsync(cancellationToken);

        if (clusterAdmin == null)
            return false;

        dbContext.ClusterAdmins.Remove(clusterAdmin);
        await dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }
}
