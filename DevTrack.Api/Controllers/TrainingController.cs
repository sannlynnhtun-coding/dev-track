using DevTrack.Domain.Features.Training;
using DevTrack.Domain.Features.Training.Models;
using DevTrack.Shared;
using Microsoft.AspNetCore.Mvc;

namespace DevTrack.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TrainingController : ControllerBase
{
    private readonly ITrainingService _trainingService;

    public TrainingController(ITrainingService trainingService)
    {
        _trainingService = trainingService;
    }

    [HttpGet("batch/{batchId}/class-days")]
    public async Task<IActionResult> GetClassDays(int batchId)
    {
        var result = await _trainingService.GetClassDaysAsync(batchId);
        return Ok(result);
    }

    [HttpGet("batch/{batchId}/schedule")]
    public async Task<IActionResult> GetSchedule(int batchId, [FromQuery] PaginationRequest request)
    {
        var result = await _trainingService.GetScheduleAsync(batchId, request);
        return Ok(result);
    }

    [HttpGet("batch/{batchId}/attendance/{date}")]
    public async Task<IActionResult> GetAttendance(int batchId, string date)
    {
        if (DateOnly.TryParse(date, out var targetDate))
        {
            var result = await _trainingService.GetAttendanceForDateAsync(batchId, targetDate);
            return Ok(result);
        }
        return BadRequest("Invalid date format.");
    }

    [HttpPost("attendance/bulk")]
    public async Task<IActionResult> MarkBulkAttendance(BulkAttendanceRequest request)
    {
        var result = await _trainingService.MarkBulkAttendanceAsync(request);
        return Ok(result);
    }

    [HttpGet("batch/{batchId}/summary")]
    public async Task<IActionResult> GetAttendanceSummary(int batchId, [FromQuery] PaginationRequest request)
    {
        var result = await _trainingService.GetAttendanceSummaryAsync(batchId, request);
        return Ok(result);
    }
}
