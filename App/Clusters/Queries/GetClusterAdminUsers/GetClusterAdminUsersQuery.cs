using App.Common.Security;

namespace App.Clusters.Queries.GetClusterAdminUsers;

[Authorize]
public record GetClusterAdminUsersQuery : IRequest<List<ClusterAdminUserDto>>;
