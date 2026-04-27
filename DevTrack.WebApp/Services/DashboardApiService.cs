using DevTrack.Domain.Features.Dashboard;
using DevTrack.Domain.Features.Dashboard.Models;
using DevTrack.Shared;

namespace DevTrack.WebApp.Services;

public class DashboardApiService : IDashboardService
{
    private readonly IDashboardApiClient _api;

    public DashboardApiService(IDashboardApiClient api)
    {
        _api = api;
    }

    public Task<Result<DashboardResponse>> GetDashboardDataAsync() => _api.GetDashboardDataAsync();
}
