using DevTrack.Domain.Features.Training.Models;
using DevTrack.Shared;

namespace DevTrack.Domain.Features.Training;

public interface ITrainingService
{
    Task<Result<List<DateOnly>>> GetClassDaysAsync(int batchId);
    Task<PagedResult<TrainingCalendarResponse>> GetScheduleAsync(int batchId, PaginationRequest request);
    Task<Result<BulkAttendanceRequest>> GetAttendanceForDateAsync(int batchId, DateOnly date);
    Task<Result> MarkBulkAttendanceAsync(BulkAttendanceRequest request);
    Task<PagedResult<AttendanceSummaryResponse>> GetAttendanceSummaryAsync(int batchId, PaginationRequest request);
}
