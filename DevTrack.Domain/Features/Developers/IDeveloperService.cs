using DevTrack.Domain.Features.Developers.Models;
using DevTrack.Shared;

namespace DevTrack.Domain.Features.Developers;

public interface IDeveloperService
{
    Task<PagedResult<DeveloperResponse>> GetDevelopersAsync(PaginationRequest request);
    Task<Result<DeveloperResponse>> CreateDeveloperAsync(DeveloperRequest request);
}
