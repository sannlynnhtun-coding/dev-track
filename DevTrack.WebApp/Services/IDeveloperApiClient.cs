using DevTrack.Domain.Features.Developers.Models;
using DevTrack.Shared;
using Refit;

namespace DevTrack.WebApp.Services;

public interface IDeveloperApiClient
{
    [Get("/api/developers")]
    Task<PagedResult<DeveloperResponse>> GetDevelopersAsync(PaginationRequest request);

    [Post("/api/developers")]
    Task<Result<DeveloperResponse>> CreateDeveloperAsync(DeveloperRequest request);
}
