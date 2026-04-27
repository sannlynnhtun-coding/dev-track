using DevTrack.Domain.Features.Training.Models;
using DevTrack.Shared;
using Refit;

namespace DevTrack.WebApp.Services;

public interface ITrainingApiClient
{
    [Get("/api/training/batch/{batchId}/class-days")]
    Task<Result<List<DateOnly>>> GetClassDaysAsync(int batchId);

    [Get("/api/training/batch/{batchId}/schedule")]
    Task<Result<List<TrainingCalendarResponse>>> GetScheduleAsync(int batchId);

    [Get("/api/training/batch/{batchId}/attendance/{date}")]
    Task<Result<BulkAttendanceRequest>> GetAttendanceForDateAsync(int batchId, string date);

    [Post("/api/training/attendance/bulk")]
    Task<Result> MarkBulkAttendanceAsync(BulkAttendanceRequest request);

    [Get("/api/training/batch/{batchId}/summary")]
    Task<Result<List<AttendanceSummaryResponse>>> GetAttendanceSummaryAsync(int batchId);
}
