using DevTrack.Domain.Features.Dashboard.Models;
using DevTrack.Shared;
using Refit;

namespace DevTrack.WebApp.Services;

public interface IDashboardApiClient
{
    [Get("/api/dashboard")]
    Task<Result<DashboardResponse>> GetDashboardDataAsync();

    [Get("/api/dashboard/report")]
    Task<Result<List<BatchSummaryModel>>> GetDashboardReportAsync();
}
