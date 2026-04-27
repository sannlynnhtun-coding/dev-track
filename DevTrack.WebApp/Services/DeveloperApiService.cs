using DevTrack.Domain.Features.Developers;
using DevTrack.Domain.Features.Developers.Models;
using DevTrack.Shared;

namespace DevTrack.WebApp.Services;

public class DeveloperApiService : IDeveloperService
{
    private readonly IDeveloperApiClient _api;

    public DeveloperApiService(IDeveloperApiClient api)
    {
        _api = api;
    }

    public Task<PagedResult<DeveloperResponse>> GetDevelopersAsync(PaginationRequest request) => _api.GetDevelopersAsync(request);
    public Task<Result<DeveloperResponse>> CreateDeveloperAsync(DeveloperRequest request) => _api.CreateDeveloperAsync(request);
}
