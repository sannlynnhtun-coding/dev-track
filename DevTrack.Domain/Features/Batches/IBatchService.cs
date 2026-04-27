using DevTrack.Domain.Features.Batches.Models;
using DevTrack.Shared;

namespace DevTrack.Domain.Features.Batches;

public interface IBatchService
{
    Task<PagedResult<BatchResponse>> GetBatchesAsync(PaginationRequest request);
    Task<Result<BatchResponse>> GetBatchByIdAsync(int id);
    Task<Result<BatchResponse>> CreateBatchAsync(BatchRequest request);
    Task<Result<List<BatchAssignmentModel>>> GetBatchDevelopersAsync(int batchId);
    Task<Result> UpdateBatchAssignmentsAsync(int batchId, List<int> selectedDeveloperIds);
}
