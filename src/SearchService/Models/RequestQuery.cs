namespace SearchService.Models;

public class RequestQuery
{
    private int MAX_PAGE_SIZE { get; set; } = 50;
    public int PageNumber { get; set; } = 1;
    private int _pageSize { get; set; } = 5;

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > MAX_PAGE_SIZE ? MAX_PAGE_SIZE : value;
    }
    public string SearchTerm { get; set; }
}
