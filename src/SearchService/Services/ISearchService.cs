using Carsties.Shared.Models.Core;
using SearchService.Entities;
using SearchService.Models;

namespace SearchService.Services;

public interface ISearchService
{
    Task<Result<Pagination<Item>>> SearchAsync(RequestQuery query, string correlationII);
}
