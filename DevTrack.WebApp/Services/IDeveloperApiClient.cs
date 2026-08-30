using DevTrack.Domain.Features.Developers.Models;
using DevTrack.Shared;

namespace DevTrack.WebApp.Services;

public interface IDeveloperApiClient
{
    Task<PagedResult<DeveloperResponse>> GetDevelopersAsync(PaginationRequest request);

    Task<Result<DeveloperDetailResponse>> GetDeveloperByIdAsync(int id);

    Task<Result<DeveloperResponse>> CreateDeveloperAsync(DeveloperRequest request);
}
