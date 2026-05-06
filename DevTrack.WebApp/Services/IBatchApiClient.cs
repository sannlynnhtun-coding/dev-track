using DevTrack.Domain.Features.Batches.Models;
using DevTrack.Shared;
using Refit;

namespace DevTrack.WebApp.Services;

public interface IBatchApiClient
{
    [Get("/api/batches")]
    Task<PagedResult<BatchResponse>> GetBatchesAsync(PaginationRequest request);

    [Get("/api/batches/{id}")]
    Task<Result<BatchResponse>> GetBatchByIdAsync(int id);

    [Post("/api/batches")]
    Task<Result<BatchResponse>> CreateBatchAsync([Body] BatchRequest request);

    [Get("/api/batches/{id}/developers")]
    Task<Result<List<BatchAssignmentModel>>> GetBatchDevelopersAsync(int id);

    [Post("/api/batches/{id}/assignments")]
    Task<Result> UpdateBatchAssignmentsAsync(int id, [Body] List<int> selectedDeveloperIds);
}
