using DevTrack.Domain.Features.Batches.Models;
using DevTrack.Shared;

namespace DevTrack.WebApp.Services;

public interface IBatchApiClient
{
    Task<PagedResult<BatchResponse>> GetBatchesAsync(PaginationRequest request);

    Task<Result<BatchResponse>> GetBatchByIdAsync(int id);

    Task<Result<BatchResponse>> CreateBatchAsync(BatchRequest request);

    Task<Result<List<BatchAssignmentModel>>> GetBatchDevelopersAsync(int id);

    Task<Result> UpdateBatchAssignmentsAsync(int id, List<int> selectedDeveloperIds);
}
