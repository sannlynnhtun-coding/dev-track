using DevTrack.Domain.Features.Training;
using DevTrack.Domain.Features.Training.Models;
using DevTrack.Shared;

namespace DevTrack.WebApp.Services;

public class TrainingApiService : ITrainingService
{
    private readonly ITrainingApiClient _api;

    public TrainingApiService(ITrainingApiClient api)
    {
        _api = api;
    }

    public Task<Result<List<DateOnly>>> GetClassDaysAsync(int batchId) => _api.GetClassDaysAsync(batchId);

    public Task<Result<List<TrainingCalendarResponse>>> GetScheduleAsync(int batchId) => _api.GetScheduleAsync(batchId);
    
    public Task<Result<BulkAttendanceRequest>> GetAttendanceForDateAsync(int batchId, DateOnly date) 
        => _api.GetAttendanceForDateAsync(batchId, date.ToString("yyyy-MM-dd"));

    public Task<Result> MarkBulkAttendanceAsync(BulkAttendanceRequest request) => _api.MarkBulkAttendanceAsync(request);
    
    public Task<Result<List<AttendanceSummaryResponse>>> GetAttendanceSummaryAsync(int batchId) => _api.GetAttendanceSummaryAsync(batchId);
}
