using DevTrack.Domain.Features.Batches;
using DevTrack.Domain.Features.Batches.Models;
using DevTrack.Shared;

namespace DevTrack.WebApp.Services;

public class BatchApiService : IBatchService
{
    private readonly IBatchApiClient _api;

    public BatchApiService(IBatchApiClient api)
    {
        _api = api;
    }

    public Task<Result<List<BatchResponse>>> GetBatchesAsync() => _api.GetBatchesAsync();
    public Task<Result<BatchResponse>> GetBatchByIdAsync(int id) => _api.GetBatchByIdAsync(id);
    public Task<Result<BatchResponse>> CreateBatchAsync(BatchRequest request) => _api.CreateBatchAsync(request);
    public Task<Result<List<BatchAssignmentModel>>> GetBatchDevelopersAsync(int batchId) => _api.GetBatchDevelopersAsync(batchId);
    public Task<Result> UpdateBatchAssignmentsAsync(int batchId, List<int> selectedDeveloperIds) => _api.UpdateBatchAssignmentsAsync(batchId, selectedDeveloperIds);
}
