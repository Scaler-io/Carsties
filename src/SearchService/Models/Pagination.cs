namespace SearchService.Models;

public class Pagination<T> where T : class
{
    public Pagination(int pageSize, int pageNumber, int pageCount, long total, IReadOnlyList<T> data)
    {
        PageSize = pageSize;
        PageNumber = pageNumber;
        PageCount = pageCount;
        Total = total;
        Data = data;
    }

    public int PageSize { get; set; }
    public int PageNumber { get; set; }
    public int PageCount { get; set; }
    public long Total { get; set; }
    public IReadOnlyList<T> Data { get; set; }


}
