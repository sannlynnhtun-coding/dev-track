using DevTrack.Domain.Features.Training.Models;
using DevTrack.Shared;
using Refit;

namespace DevTrack.WebApp.Services;

public interface ITrainingApiClient
{
    [Get("/api/training/batch/{batchId}/class-days")]
    Task<Result<List<DateOnly>>> GetClassDaysAsync(int batchId);

    [Get("/api/training/batch/{batchId}/schedule")]
    Task<PagedResult<TrainingCalendarResponse>> GetScheduleAsync(int batchId, PaginationRequest request);

    [Get("/api/training/batch/{batchId}/attendance/{date}")]
    Task<Result<BulkAttendanceRequest>> GetAttendanceForDateAsync(int batchId, string date);

    [Post("/api/training/attendance/bulk")]
    Task<Result> MarkBulkAttendanceAsync(BulkAttendanceRequest request);

    [Get("/api/training/batch/{batchId}/summary")]
    Task<PagedResult<AttendanceSummaryResponse>> GetAttendanceSummaryAsync(int batchId, PaginationRequest request);

    [Get("/api/training/batch/{batchId}/summary/full")]
    Task<Result<List<AttendanceSummaryResponse>>> GetFullAttendanceSummaryAsync(int batchId);
}
