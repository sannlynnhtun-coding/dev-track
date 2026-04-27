using DevTrack.Domain.Features.Developers.Models;
using DevTrack.Shared;

namespace DevTrack.Domain.Features.Developers;

public interface IDeveloperService
{
    Task<Result<List<DeveloperResponse>>> GetDevelopersAsync();
    Task<Result<DeveloperResponse>> CreateDeveloperAsync(DeveloperRequest request);
}
