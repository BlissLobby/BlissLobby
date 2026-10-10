namespace App.Clusters.Queries.GetClustersForDisplay;

public class GetClustersForDisplayQueryValidator : AbstractValidator<GetClustersForDisplayQuery>
{
    public GetClustersForDisplayQueryValidator()
    {
        RuleFor(x => x.Query).NotNull().WithMessage("Query is required");
        RuleFor(x => x.Query.Page).GreaterThan(0).WithMessage("Page must be greater than 0");
        RuleFor(x => x.Query.PageSize).GreaterThan(0).WithMessage("Page size must be greater than 0");
    }
}
