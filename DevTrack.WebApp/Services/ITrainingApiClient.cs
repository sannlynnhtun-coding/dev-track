using DevTrack.Domain.Features.Training.Models;
using DevTrack.Shared;

namespace DevTrack.WebApp.Services;

public interface ITrainingApiClient
{
    Task<Result<List<DateOnly>>> GetClassDaysAsync(int batchId);

    Task<PagedResult<TrainingCalendarResponse>> GetScheduleAsync(int batchId, PaginationRequest request);

    Task<Result<BulkAttendanceRequest>> GetAttendanceForDateAsync(int batchId, string date);

    Task<Result> MarkBulkAttendanceAsync(BulkAttendanceRequest request);

    Task<PagedResult<AttendanceSummaryResponse>> GetAttendanceSummaryAsync(int batchId, PaginationRequest request);

    Task<Result<List<AttendanceSummaryResponse>>> GetFullAttendanceSummaryAsync(int batchId);
}
