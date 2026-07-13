namespace ToDoApp.Domain.Dtos;

/// <summary>
/// A single page of results plus the info the client needs to render paging
/// (e.g. "page 2 of 5"). Generic, so it works for any item type T.
/// </summary>
public class PagedResult<T>
{
    /// <summary>The items on the current page.</summary>
    public IEnumerable<T> Items { get; set; } = new List<T>();

    /// <summary>Current page number (1-based).</summary>
    public int Page { get; set; }

    /// <summary>How many items per page.</summary>
    public int PageSize { get; set; }

    /// <summary>Total number of items across all pages.</summary>
    public int TotalCount { get; set; }

    /// <summary>Total number of pages (calculated from TotalCount and PageSize).</summary>
    public int TotalPages { get; set; }
}
