using App.Common.Interfaces;
using App.Common.Security;
using Domain.Constants;

namespace App.Clusters.Commands.DeleteCluster;

[Authorize(Roles = Roles.Administrator)]
public record DeleteClusterCommand(string Id) : IRequest<bool>;

public class DeleteClusterCommandHandler(IApplicationDbContext dbContext) : IRequestHandler<DeleteClusterCommand, bool>
{
    public async Task<bool> Handle(DeleteClusterCommand request, CancellationToken cancellationToken)
    {
        var cluster = await dbContext.Clusters.FindAsync([request.Id], cancellationToken);

        if (cluster == null)
            return false;

        dbContext.Clusters.Remove(cluster);
        await dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }
}
