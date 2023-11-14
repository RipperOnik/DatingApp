
using Microsoft.EntityFrameworkCore;

namespace API.Helpers;

public class PagedList<T> : List<T>
{
    public PagedList(IEnumerable<T> items, int count, int pageNumber, int pageSize)
    {
        CurrentPage = pageNumber;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        PageSize = pageSize;
        TotalCount = count;
        this.AddRange(items);
    }

    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public static async Task<PagedList<T>> CreateAsync(
        IQueryable<T> source, // this does not execute in DB
        int pageNumber,
        int pageSize
    )
    {
        var count = await source.CountAsync(); // this executes on DB
        var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(); // this executes on DB

        return new PagedList<T>(items, count, pageNumber, pageSize);
    }

}
