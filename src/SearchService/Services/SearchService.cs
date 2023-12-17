using Carsties.Shared.Extensions.Logger;
using Carsties.Shared.Models.Core;
using Carsties.Shared.Models.Enums;
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

        var query = DB.PagedSearch<Item>();
        query.Sort(x => x.Ascending(a => a.Make));

        if (!string.IsNullOrEmpty(queryParam.SearchTerm))
        {
            query.Match(Search.Full, queryParam.SearchTerm).SortByTextScore();
        }

        query.PageNumber(queryParam.PageNumber);
        query.PageSize(queryParam.PageSize);

        var paginatedResult = await query.ExecuteAsync();

        if (!paginatedResult.Results.Any())
        {
            _logger.Here().WithCorrelationId(correlationId).Warning("No item were found");
            return Result<Pagination<Item>>.Failure(ErrorCodes.NotFound);
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
}
