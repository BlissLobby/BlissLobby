using App.Common.Pagination;

namespace App.Users.Queries.GetUsers;

public class GetUsersQueryValidator : AbstractValidator<GetUsersQuery>
{
    public GetUsersQueryValidator()
    {
        RuleFor(v => v.Query)
            .ValidatePagination();
    }
}