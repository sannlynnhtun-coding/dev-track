using DevTrack.Domain.Features.Batches.Models;
using DevTrack.Shared;

namespace DevTrack.WebApp.Services;

public class BatchApiClient : ApiClientBase, IBatchApiClient
{
    public BatchApiClient(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
    {
    }

    public Task<PagedResult<BatchResponse>> GetBatchesAsync(PaginationRequest request)
        => GetAsync<PagedResult<BatchResponse>>(WithPagination("/api/batches", request));

    public Task<Result<BatchResponse>> GetBatchByIdAsync(int id)
        => GetAsync<Result<BatchResponse>>($"/api/batches/{id}");

    public Task<Result<BatchResponse>> CreateBatchAsync(BatchRequest request)
        => PostAsync<BatchRequest, Result<BatchResponse>>("/api/batches", request);

    public Task<Result<List<BatchAssignmentModel>>> GetBatchDevelopersAsync(int id)
        => GetAsync<Result<List<BatchAssignmentModel>>>($"/api/batches/{id}/developers");

    public Task<Result> UpdateBatchAssignmentsAsync(int id, List<int> selectedDeveloperIds)
        => PostAsync<List<int>, Result>($"/api/batches/{id}/assignments", selectedDeveloperIds);
}
