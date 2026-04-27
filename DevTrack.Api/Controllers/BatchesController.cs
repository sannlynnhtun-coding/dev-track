using DevTrack.Domain.Features.Batches;
using DevTrack.Domain.Features.Batches.Models;
using DevTrack.Shared;
using Microsoft.AspNetCore.Mvc;

namespace DevTrack.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BatchesController : ControllerBase
{
    private readonly IBatchService _batchService;

    public BatchesController(IBatchService batchService)
    {
        _batchService = batchService;
    }

    [HttpGet]
    public async Task<IActionResult> GetBatches([FromQuery] PaginationRequest request)
    {
        var result = await _batchService.GetBatchesAsync(request);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetBatchById(int id)
    {
        var result = await _batchService.GetBatchByIdAsync(id);
        return Ok(result);
    }

    [HttpGet("{id}/developers")]
    public async Task<IActionResult> GetBatchDevelopers(int id)
    {
        var result = await _batchService.GetBatchDevelopersAsync(id);
        return Ok(result);
    }

    [HttpPost("{id}/assignments")]
    public async Task<IActionResult> UpdateAssignments(int id, [FromBody] List<int> selectedDeveloperIds)
    {
        var result = await _batchService.UpdateBatchAssignmentsAsync(id, selectedDeveloperIds);
        return Ok(result);
    }
}
