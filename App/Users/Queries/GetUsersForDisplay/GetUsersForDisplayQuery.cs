using App.Common.Pagination;
using App.Common.Security;

namespace App.Users.Queries.GetUsersForDisplay;

[Authorize]
public record GetUsersForDisplayQuery(PaginationQuery Query) : IRequest<PaginationResponse<UserListDto>>;
