using App.Common.Pagination;

namespace App.Users.Queries.GetUsersForDisplay;

public class GetUsersForDisplayQueryValidator : AbstractValidator<GetUsersForDisplayQuery>
{
    public GetUsersForDisplayQueryValidator()
    {
        RuleFor(v => v.Query)
            .ValidatePagination();
    }
}