namespace App.Common.Pagination;

public record PaginationResponse<T>(IEnumerable<T> Items, int TotalCount);
