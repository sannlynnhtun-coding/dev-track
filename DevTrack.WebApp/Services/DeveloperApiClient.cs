using DevTrack.Domain.Features.Developers.Models;
using DevTrack.Shared;

namespace DevTrack.WebApp.Services;

public class DeveloperApiClient : ApiClientBase, IDeveloperApiClient
{
    public DeveloperApiClient(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
    {
    }

    public Task<PagedResult<DeveloperResponse>> GetDevelopersAsync(PaginationRequest request)
        => GetAsync<PagedResult<DeveloperResponse>>(WithPagination("/api/developers", request));

    public Task<Result<DeveloperDetailResponse>> GetDeveloperByIdAsync(int id)
        => GetAsync<Result<DeveloperDetailResponse>>($"/api/developers/{id}");

    public Task<Result<DeveloperResponse>> CreateDeveloperAsync(DeveloperRequest request)
        => PostAsync<DeveloperRequest, Result<DeveloperResponse>>("/api/developers", request);
}
