namespace App.Common.Pagination;

public record PaginationQuery(int Page, int PageSize, Dictionary<string, string> Filters, string? SortBy, bool SortDesc, bool RequestNewCount);
