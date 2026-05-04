using App.Common.Pagination;
using App.Common.Security;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace App.Users.Queries.GetUsers;

[Authorize]
public record GetUsersQuery(PaginationQuery Query) : IRequest<PaginationResponse<ApplicationUser>>;

public class GetUsersQueryHandler(UserManager<ApplicationUser> userManager) : IRequestHandler<GetUsersQuery, PaginationResponse<ApplicationUser>>
{
    public async Task<PaginationResponse<ApplicationUser>> Handle(GetUsersQuery usersQuery, CancellationToken cancellationToken)
    {
        var req = usersQuery.Query;
        IQueryable<ApplicationUser> users = userManager.Users;

        // filter the user as per each of req.Filters
        users = users.ApplyFilters(req.Filters);

        // sort the user as per req.SortBy and req.SortDescending
        users = users.ApplySort($"{req.SortBy} {(req.SortDesc ? "desc" : "asc")}");
        
        // get paginated results
        users = users.ApplyPagination(req.Page, req.PageSize);
        int totalCount = -1;
        if (req.RequestNewCount)
        {
            totalCount = await userManager.Users.CountAsync(cancellationToken);
        }
        return new PaginationResponse<ApplicationUser>(users, totalCount);
    }
}
