using DevTrack.Domain.Features.Dashboard.Models;
using DevTrack.Shared;

namespace DevTrack.Domain.Features.Dashboard;

public interface IDashboardService
{
    Task<Result<DashboardResponse>> GetDashboardDataAsync();
}
