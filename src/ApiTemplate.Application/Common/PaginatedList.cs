namespace ApiTemplate.Application.Common;

public class PaginatedList<T>
{
    public int? Offset { get; }
    public int? Limit { get; }
    public int Total { get; }
    public List<T> Items { get; }
    
    public PaginatedList(List<T> items, int? offset, int? limit, int total)
    {
        Items = items;
        Offset = offset;
        Limit = limit;
        Total = total;
    }
    
    public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int? offset = null, int? limit = null, CancellationToken cancellationToken = default)
    {
        var total = await source.CountAsync(cancellationToken);
        var items = await source.Skip(offset ?? 0).Take(limit ?? total).ToListAsync(cancellationToken);
        return new PaginatedList<T>(items, offset, limit, total);
    }
}