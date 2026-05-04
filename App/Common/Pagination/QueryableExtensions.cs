using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Reflection;

namespace App.Common.Pagination;

public static class QueryableExtensions
{
    public static IQueryable<T> ApplyPagination<T>(this IQueryable<T> query, int pageNumber, int pageSize)
    {
        return query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize);
    }

    public static IQueryable<T> ApplySort<T>(this IQueryable<T> query, string? sortBy) where T : class
    {
        if (string.IsNullOrWhiteSpace(sortBy))
            return query;

        // TODO sanitize sortBy to prevent injection attacks

        var allowedProperties = typeof(T)
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Select(p => p.Name)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        var sortExpressions = new List<string>();

        foreach (var part in sortBy.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
        {
            var tokens = part.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (tokens.Length == 0 || !allowedProperties.Contains(tokens[0]))
                continue;

            var direction = tokens.Length > 1 && tokens[1].Equals("desc", StringComparison.OrdinalIgnoreCase)
                ? "descending"
                : "ascending";

            sortExpressions.Add($"{tokens[0]} {direction}");
        }

        return sortExpressions.Count > 0
            ? query.OrderBy(string.Join(", ", sortExpressions))
            : query;
    }

    public static IQueryable<T> ApplyFilters<T>(this IQueryable<T> query, IDictionary<string, string> filters)
    {
        var allowedProperties = typeof(T)
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Select(p => p.Name)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        // TODO sanitize filters keys and values to prevent injection attacks

        foreach (var filter in filters)
        {
            if (string.IsNullOrEmpty(filter.Value)) continue;
            if (!allowedProperties.Contains(filter.Key)) continue;
            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(parameter, filter.Key);
            var propertyType = ((PropertyInfo)property.Member).PropertyType;

            Expression comparison;

            if (propertyType == typeof(string))
            {
                // For strings, use: x.Property.Contains(value)
                var method = typeof(string).GetMethod("Contains", [typeof(string)]);
                var value = Expression.Constant(filter.Value, typeof(string));
                comparison = Expression.Call(property, method, value);
            }
            else
            {
                // For other types, use: x.Property == convertedValue
                var convertedValue = Convert.ChangeType(filter.Value, propertyType);
                var constant = Expression.Constant(convertedValue);
                comparison = Expression.Equal(property, constant);
            }

            var lambda = Expression.Lambda<Func<T, bool>>(comparison, parameter);
            query = query.Where(lambda);
        }

        return query;
    }
}

