using App.Common.Pagination;
using Domain.Constants;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace App.Users.Queries.GetUsersForDisplay;

public class GetUsersForDisplayQueryHandler(UserManager<ApplicationUser> userManager) : IRequestHandler<GetUsersForDisplayQuery, PaginationResponse<UserListDto>>
{
    public async Task<PaginationResponse<UserListDto>> Handle(GetUsersForDisplayQuery usersQuery, CancellationToken cancellationToken)
    {
        var req = usersQuery.Query;
        IQueryable<ApplicationUser> users = userManager.Users;

        // filter the user as per each of req.Filters
        users = users.ApplyFilters(req.Filters);

        // sort the user as per req.SortBy and req.SortDescending
        users = users.ApplySort($"{req.SortBy} {(req.SortDesc ? "desc" : "asc")}");

        // get paginated results
        users = users.ApplyPagination(req.Page, req.PageSize);

        List<UserListDto> userDtos = [];

        foreach (var user in users)
        {
            var userDto = new UserListDto
            {
                Id = user.Id,
                UserName = user.UserName ?? "",
                DisplayName = user.DisplayName,
                Email = user.Email ?? "",
                EmailConfirmed = user.EmailConfirmed,
                Roles = [.. await userManager.GetRolesAsync(user)]
            };

            // do not display administrator user details
            if (userDto.Roles.Contains(Roles.Administrator)) continue;

            userDtos.Add(userDto);
        }

        int totalCount = -1;
        if (req.RequestNewCount)
        {
            totalCount = await userManager.Users.CountAsync(cancellationToken);
        }
        return new PaginationResponse<UserListDto>(userDtos, totalCount);
    }
}
