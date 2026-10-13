using App.Common.Interfaces;
using Domain.Constants;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace App.Clusters.Commands.CreateClusterAdmin;

public class CreateClusterAdminCommandHandler(
    IApplicationDbContext dbContext,
    UserManager<ApplicationUser> userManager) : IRequestHandler<CreateClusterAdminCommand, bool>
{
    public async Task<bool> Handle(CreateClusterAdminCommand request, CancellationToken cancellationToken)
    {
        var clusterExists = await dbContext.Clusters
            .AnyAsync(cluster => cluster.Id == request.ClusterId, cancellationToken);
        var admin = await userManager.FindByIdAsync(request.AdminId);

        if (!clusterExists || admin == null)
            return false;

        if (!await userManager.IsInRoleAsync(admin, Roles.ClusterAdmin))
            return false;

        var alreadyAssigned = await dbContext.ClusterAdmins
            .AnyAsync(clusterAdmin =>
                clusterAdmin.ClusterId == request.ClusterId &&
                clusterAdmin.AdminId == request.AdminId,
                cancellationToken);

        if (alreadyAssigned)
            return false;

        dbContext.ClusterAdmins.Add(new ClusterAdmin
        {
            ClusterId = request.ClusterId,
            AdminId = request.AdminId
        });

        await dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }
}
