using DevTrack.Domain.Features.Training.Models;
using DevTrack.Shared;

namespace DevTrack.Domain.Features.Training;

public interface ITrainingService
{
    Task<Result<List<DateOnly>>> GetClassDaysAsync(int batchId);
    Task<Result<List<TrainingCalendarResponse>>> GetScheduleAsync(int batchId);
    Task<Result<BulkAttendanceRequest>> GetAttendanceForDateAsync(int batchId, DateOnly date);
    Task<Result> MarkBulkAttendanceAsync(BulkAttendanceRequest request);
    Task<Result<List<AttendanceSummaryResponse>>> GetAttendanceSummaryAsync(int batchId);
}
