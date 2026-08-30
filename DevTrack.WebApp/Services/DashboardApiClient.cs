using DevTrack.Domain.Features.Dashboard.Models;
using DevTrack.Shared;

namespace DevTrack.WebApp.Services;

public class DashboardApiClient : ApiClientBase, IDashboardApiClient
{
    public DashboardApiClient(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
    {
    }

    public Task<Result<DashboardResponse>> GetDashboardDataAsync()
        => GetAsync<Result<DashboardResponse>>("/api/dashboard");

    public Task<Result<List<BatchSummaryModel>>> GetDashboardReportAsync()
        => GetAsync<Result<List<BatchSummaryModel>>>("/api/dashboard/report");
}
