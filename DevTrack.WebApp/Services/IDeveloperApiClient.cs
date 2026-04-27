using DevTrack.Domain.Features.Developers.Models;
using DevTrack.Shared;
using Refit;

namespace DevTrack.WebApp.Services;

public interface IDeveloperApiClient
{
    [Get("/api/developers")]
    Task<Result<List<DeveloperResponse>>> GetDevelopersAsync();

    [Post("/api/developers")]
    Task<Result<DeveloperResponse>> CreateDeveloperAsync(DeveloperRequest request);
}
