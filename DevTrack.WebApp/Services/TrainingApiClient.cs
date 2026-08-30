using DevTrack.Domain.Features.Training.Models;
using DevTrack.Shared;

namespace DevTrack.WebApp.Services;

public class TrainingApiClient : ApiClientBase, ITrainingApiClient
{
    public TrainingApiClient(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
    {
    }

    public Task<Result<List<DateOnly>>> GetClassDaysAsync(int batchId)
        => GetAsync<Result<List<DateOnly>>>($"/api/training/batch/{batchId}/class-days");

    public Task<PagedResult<TrainingCalendarResponse>> GetScheduleAsync(int batchId, PaginationRequest request)
        => GetAsync<PagedResult<TrainingCalendarResponse>>(WithPagination($"/api/training/batch/{batchId}/schedule", request));

    public Task<Result<BulkAttendanceRequest>> GetAttendanceForDateAsync(int batchId, string date)
        => GetAsync<Result<BulkAttendanceRequest>>($"/api/training/batch/{batchId}/attendance/{date}");

    public Task<Result> MarkBulkAttendanceAsync(BulkAttendanceRequest request)
        => PostAsync<BulkAttendanceRequest, Result>("/api/training/attendance/bulk", request);

    public Task<PagedResult<AttendanceSummaryResponse>> GetAttendanceSummaryAsync(int batchId, PaginationRequest request)
        => GetAsync<PagedResult<AttendanceSummaryResponse>>(WithPagination($"/api/training/batch/{batchId}/summary", request));

    public Task<Result<List<AttendanceSummaryResponse>>> GetFullAttendanceSummaryAsync(int batchId)
        => GetAsync<Result<List<AttendanceSummaryResponse>>>($"/api/training/batch/{batchId}/summary/full");
}
