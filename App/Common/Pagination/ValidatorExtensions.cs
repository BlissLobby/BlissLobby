namespace App.Common.Pagination;

public static class ValidatorExtensions
{
    public static IRuleBuilderOptions<T, PaginationQuery> ValidatePagination<T>(
        this IRuleBuilder<T, PaginationQuery> ruleBuilder)
    {
        return ruleBuilder
            .ChildRules(v =>
            {
                v.RuleFor(x => x.PageSize).LessThanOrEqualTo(100)
                .WithMessage("Page size must be less than 100");

                // TODO add validation for filters keys and values, SortBy to prevent injection attacks
            });
    }
}