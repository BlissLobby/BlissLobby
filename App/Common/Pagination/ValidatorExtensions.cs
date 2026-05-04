namespace App.Common.Pagination;

public static class ValidatorExtensions
{
    public static IRuleBuilderOptions<T, PaginationQuery> ValidatePagination<T>(
        this IRuleBuilder<T, PaginationQuery> ruleBuilder)
    {
        return ruleBuilder
            .ChildRules(v =>
            {
                v.RuleFor(x => x.Page).GreaterThanOrEqualTo(1)
                .WithMessage("Page number must be at least 1.");

                v.RuleFor(x => x.PageSize).InclusiveBetween(1, 100)
                .WithMessage("Page size must be at least 1.");

                // TODO add validation for filters keys and values, SortBy to prevent injection attacks
            });
    }
}