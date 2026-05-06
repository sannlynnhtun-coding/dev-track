using DevTrack.Domain.Features.Developers.Models;
using DevTrack.Shared;
using Refit;

namespace DevTrack.WebApp.Services;

public interface IDeveloperApiClient
{
    [Get("/api/developers")]
    Task<PagedResult<DeveloperResponse>> GetDevelopersAsync(PaginationRequest request);

    [Get("/api/developers/{id}")]
    Task<Result<DeveloperDetailResponse>> GetDeveloperByIdAsync(int id);

    [Post("/api/developers")]
    Task<Result<DeveloperResponse>> CreateDeveloperAsync([Body] DeveloperRequest request);
}
