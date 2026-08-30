using DevTrack.Domain.Features.Dashboard.Models;
using DevTrack.Shared;

namespace DevTrack.WebApp.Services;

public interface IDashboardApiClient
{
    Task<Result<DashboardResponse>> GetDashboardDataAsync();

    Task<Result<List<BatchSummaryModel>>> GetDashboardReportAsync();
}
