using App.Common.Interfaces;

namespace App.Clusters.Commands.CreateCluster;

public class CreateClusterCommandHandler(IApplicationDbContext dbContext) : IRequestHandler<CreateClusterCommand, string>
{
    public async Task<string> Handle(CreateClusterCommand request, CancellationToken cancellationToken)
    {
        var cluster = new Domain.Entities.Cluster
        {
            Name = request.Name,
            Description = request.Description
        };

        dbContext.Clusters.Add(cluster);
        await dbContext.SaveChangesAsync(cancellationToken);

        return cluster.Id;
    }
}
