using Carsties.Shared.Extensions.Logger;
using Carsties.Shared.Models.Core;
using MongoDB.Entities;
using SearchService.Entities;
using SearchService.Models;

namespace SearchService.Services;

public class SearchService : ISearchService
{
    private readonly ILogger _logger;

    public SearchService(ILogger logger)
    {
        _logger = logger;
    }

    public async Task<Result<Pagination<Item>>> SearchAsync(RequestQuery queryParam, string correlationId)
    {
        _logger.Here().MethodEnterd();
        _logger.Here()
            .WithCorrelationId(correlationId)
            .Information("Request - search items with {searchTerms}", queryParam.SearchTerm);

        var query = DB.PagedSearch<Item, Item>();
        query.Sort(x => x.Ascending(a => a.Make));

        ApplyFullTextSearch(query, queryParam.SearchTerm);
        ApplyPaging(query, queryParam.PageNumber, queryParam.PageSize);
        ApplyOrderby(query, queryParam.OrderBy);
        ApplyFilter(query, queryParam.FilterBy);

        if (!string.IsNullOrEmpty(queryParam.Seller))
        {
            query.Match(x => x.Seller == queryParam.Seller);
        }
        if (!string.IsNullOrEmpty(queryParam.Winner))
        {
            query.Match(x => x.Winner == queryParam.Winner);
        }

        var paginatedResult = await query.ExecuteAsync();

        if (!paginatedResult.Results.Any())
        {
            _logger.Here().WithCorrelationId(correlationId).Warning("No item were found");
            return Result<Pagination<Item>>.Success(new Pagination<Item>());
        }

        _logger.Here().WithCorrelationId(correlationId).Information("Total {count} matches found.", paginatedResult.Results.Count);
        _logger.Here().MethodExited();
        return Result<Pagination<Item>>.Success(new Pagination<Item>(queryParam.PageSize,
            queryParam.PageNumber,
            paginatedResult.PageCount,
            paginatedResult.TotalCount,
            paginatedResult.Results
        ));
    }


    private void ApplyFullTextSearch(PagedSearch<Item, Item> query, string searchTerm)
    {
        if (!string.IsNullOrEmpty(searchTerm))
        {
            query.Match(Search.Full, searchTerm).SortByTextScore();
        }
    }

    private void ApplyPaging(PagedSearch<Item, Item> query, int pageNumber, int pageSize)
    {
        query.PageNumber(pageNumber);
        query.PageSize(pageSize);
    }
    private void ApplyOrderby(PagedSearch<Item, Item> query, string orderBy)
    {
        query = orderBy switch
        {
            "make" => query.Sort(x => x.Ascending(a => a.Make)),
            "new" => query.Sort(x => x.Descending(a => a.CreatedAt)),
            _ => query.Sort(x => x.Ascending(a => a.AuctionEnd))
        };
    }

    private void ApplyFilter(PagedSearch<Item, Item> query, string filterBy)
    {
        query = filterBy switch
        {
            "finished" => query.Match(x => x.AuctionEnd < DateTime.UtcNow),
            "endingSoon" => query.Match(x => x.AuctionEnd < DateTime.UtcNow.AddHours(6)
                && x.AuctionEnd > DateTime.UtcNow),
            _ => query.Match(x => x.AuctionEnd > DateTime.UtcNow)
        };
    }
}
