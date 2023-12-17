using Carsties.Shared.Extensions.Logger;
using Carsties.Shared.Models.Core;
using Carsties.Shared.Models.Enums;
using MongoDB.Entities;
using SearchService.Entities;

namespace SearchService.Services;

public class SearchService : ISearchService
{
    private readonly ILogger _logger;

    public SearchService(ILogger logger)
    {
        _logger = logger;
    }

    public async Task<Result<IReadOnlyList<Item>>> SearchAsync(string searchTerm, string correlationId)
    {
        _logger.Here().MethodEnterd();
        _logger.Here()
            .WithCorrelationId(correlationId)
            .Information("Request - search items with {searchTerms}", searchTerm);

        var query = DB.Find<Item>();
        query.Sort(x => x.Ascending(a => a.Make));

        var result = await query.ExecuteAsync();

        if (!result.Any())
        {
            _logger.Here().WithCorrelationId(correlationId).Warning("No item were found");
            return Result<IReadOnlyList<Item>>.Failure(ErrorCodes.NotFound);
        }

        _logger.Here().WithCorrelationId(correlationId).Information("Total {count} matches found.", result.Count);
        _logger.Here().MethodExited();
        return Result<IReadOnlyList<Item>>.Success(result);
    }
}
