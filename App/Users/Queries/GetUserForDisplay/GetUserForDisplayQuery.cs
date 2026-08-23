using App.Common.Security;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace App.Users.Queries.GetUserForDisplay;

[Authorize]
public record GetUserForDisplayQuery(string Id) : IRequest<UserDto?>;

public class GetUserForDisplayQueryHandler(UserManager<ApplicationUser> userManager) : IRequestHandler<GetUserForDisplayQuery, UserDto?>
{
    public async Task<UserDto?> Handle(GetUserForDisplayQuery usersQuery, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(usersQuery.Id);

        UserDto? userDto = user != null ? new UserDto()
        {
            Id = user.Id,
            UserName = user.UserName ?? "",
            DisplayName = user.DisplayName,
            Email = user.Email ?? "",
            EmailConfirmed = user.EmailConfirmed,
            Phone = user.PhoneNumber ?? "",
            PhoneConfirmed = user.PhoneNumberConfirmed,
            Roles = [.. await userManager.GetRolesAsync(user)]
        } : null;

        return userDto;
    }
}
