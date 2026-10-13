using Domain.Constants;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace App.Clusters.Queries.GetClusterAdminUsers;

public class GetClusterAdminUsersQueryHandler(UserManager<ApplicationUser> userManager)
    : IRequestHandler<GetClusterAdminUsersQuery, List<ClusterAdminUserDto>>
{
    public async Task<List<ClusterAdminUserDto>> Handle(
        GetClusterAdminUsersQuery request,
        CancellationToken cancellationToken)
    {
        var users = await userManager.GetUsersInRoleAsync(Roles.ClusterAdmin);

        return [.. users
            .OrderBy(user => user.DisplayName)
            .Select(user => new ClusterAdminUserDto
            {
                Id = user.Id,
                UserName = user.UserName ?? "",
                DisplayName = user.DisplayName
            })];
    }
}
