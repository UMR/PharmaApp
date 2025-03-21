namespace Pharmacy.Application.Wrapper;

public class PaginatedList<T>
{
    public IList<T> Items { get; set; }

    public int PageIndex { get; set; }

    public int TotalPages { get; }

    public int TotalCount { get; set; }

    public PaginatedList(IList<T> items, int count, int totalPages, int pageIndex)
    {
        Items = items;
        TotalCount = count;
        TotalPages = totalPages;
        PageIndex = pageIndex;   
    }
}
