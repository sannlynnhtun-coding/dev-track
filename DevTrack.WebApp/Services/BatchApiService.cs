using DevTrack.Domain.Features.Batches;
using DevTrack.Domain.Features.Batches.Models;
using DevTrack.Shared;
using Microsoft.Extensions.Logging;
using Refit;
using System.Text.Json;

namespace DevTrack.WebApp.Services;

public class BatchApiService : IBatchService
{
    private readonly IBatchApiClient _api;
    private readonly ILogger<BatchApiService> _logger;

    public BatchApiService(IBatchApiClient api, ILogger<BatchApiService> logger)
    {
        _api = api;
        _logger = logger;
    }

    public Task<PagedResult<BatchResponse>> GetBatchesAsync(PaginationRequest request) => _api.GetBatchesAsync(request);
    public Task<Result<BatchResponse>> GetBatchByIdAsync(int id) => _api.GetBatchByIdAsync(id);
    public async Task<Result<BatchResponse>> CreateBatchAsync(BatchRequest request)
    {
        _logger.LogInformation(
            "CreateBatch request started for BatchName={BatchName}, StartDate={StartDate}, EndDate={EndDate}",
            request.BatchName, request.StartDate, request.EndDate);

        try
        {
            var result = await _api.CreateBatchAsync(request);
            _logger.LogInformation(
                "CreateBatch request completed with IsSuccess={IsSuccess}, Message={Message}",
                result.IsSuccess, result.Message);
            return result;
        }
        catch (ValidationApiException ex)
        {
            var apiMessage = GetReadableApiError(ex.Content) ?? ex.Message;
            _logger.LogError(ex,
                "CreateBatch validation failed. BatchName={BatchName}, ApiMessage={ApiMessage}",
                request.BatchName, apiMessage);
            return Result<BatchResponse>.Failure(apiMessage);
        }
        catch (ApiException ex)
        {
            var apiMessage = GetReadableApiError(ex.Content ?? ex.Message);
            _logger.LogError(ex,
                "CreateBatch API exception. BatchName={BatchName}, StatusCode={StatusCode}, ApiMessage={ApiMessage}",
                request.BatchName, ex.StatusCode, apiMessage);
            return Result<BatchResponse>.Failure(apiMessage);
        }
    }
    public Task<Result<List<BatchAssignmentModel>>> GetBatchDevelopersAsync(int batchId) => _api.GetBatchDevelopersAsync(batchId);
    public Task<Result> UpdateBatchAssignmentsAsync(int batchId, List<int> selectedDeveloperIds) => _api.UpdateBatchAssignmentsAsync(batchId, selectedDeveloperIds);

    private static string? GetReadableApiError(Refit.ProblemDetails? details)
    {
        if (details == null)
        {
            return null;
        }

        if (!string.IsNullOrWhiteSpace(details.Detail))
        {
            return details.Detail;
        }

        if (!string.IsNullOrWhiteSpace(details.Title))
        {
            return details.Title;
        }

        return null;
    }

    private static string GetReadableApiError(string rawMessage)
    {
        if (string.IsNullOrWhiteSpace(rawMessage))
        {
            return "API validation failed.";
        }

        try
        {
            using var doc = JsonDocument.Parse(rawMessage);
            var root = doc.RootElement;

            if (root.TryGetProperty("errors", out var errors) && errors.ValueKind == JsonValueKind.Object)
            {
                var firstError = errors.EnumerateObject()
                    .SelectMany(p => p.Value.EnumerateArray().Select(v => v.GetString()))
                    .FirstOrDefault(v => !string.IsNullOrWhiteSpace(v));
                if (!string.IsNullOrWhiteSpace(firstError))
                {
                    return firstError!;
                }
            }

            if (root.TryGetProperty("title", out var title) && title.GetString() is { Length: > 0 } t)
            {
                return t;
            }
        }
        catch
        {
            // Keep original message when not JSON.
        }

        return rawMessage;
    }
}
