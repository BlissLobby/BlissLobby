using App.Common.Pagination;

namespace App.Buildings.Queries.GetBuildingsForDisplay;

public class GetBuildingsForDisplayQueryValidator : AbstractValidator<GetBuildingsForDisplayQuery>
{
    public GetBuildingsForDisplayQueryValidator()
    {
        RuleFor(x => x.Query).ValidatePagination();
    }
}
