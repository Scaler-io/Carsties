using Carsties.Shared.Models.Core;
using SearchService.Entities;

namespace SearchService.Services;

public interface ISearchService
{
    Task<Result<IReadOnlyList<Item>>> SearchAsync(string searchTerm, string correlationII);
}
