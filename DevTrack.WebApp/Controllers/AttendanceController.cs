using DevTrack.Domain.Features.Training;
using DevTrack.Domain.Features.Batches;
using DevTrack.Shared;
using Microsoft.AspNetCore.Mvc;

namespace DevTrack.WebApp.Controllers;

public class AttendanceController : Controller
{
    private readonly ITrainingService _trainingService;
    private readonly IBatchService _batchService;

    public AttendanceController(ITrainingService trainingService, IBatchService batchService)
    {
        _trainingService = trainingService;
        _batchService = batchService;
    }

    public async Task<IActionResult> Index(int? batchId, int pageAt = 1)
    {
        // For the dropdown, we want a larger list or all batches.
        var batches = await _batchService.GetBatchesAsync(new PaginationRequest { PageSize = 100 });
        ViewBag.Batches = batches.Data;

        if (batchId.HasValue)
        {
            var summaryResult = await _trainingService.GetAttendanceSummaryAsync(batchId.Value, new PaginationRequest { PageNumber = pageAt });
            ViewBag.SelectedBatchId = batchId.Value;
            return View(summaryResult);
        }

        return View(null);
    }

    public async Task<IActionResult> Schedule(int batchId, int pageAt = 1)
    {
        var scheduleResult = await _trainingService.GetScheduleAsync(batchId, new PaginationRequest { PageNumber = pageAt, PageSize = 10 });
        var batch = await _batchService.GetBatchByIdAsync(batchId);

        ViewBag.Batch = batch.Data;
        return View(scheduleResult);
    }

    public async Task<IActionResult> Mark(int batchId, string? date)
    {
        DateOnly targetDate;
        if (string.IsNullOrEmpty(date))
        {
            targetDate = DateOnly.FromDateTime(DateTime.Now);
        }
        else
        {
            targetDate = DateOnly.Parse(date);
        }

        var batchResult = await _batchService.GetBatchByIdAsync(batchId);
        var classDays = await _trainingService.GetClassDaysAsync(batchId);
        var markingData = await _trainingService.GetAttendanceForDateAsync(batchId, targetDate);

        ViewBag.Batch = batchResult.Data;
        ViewBag.ClassDays = classDays.Data;
        ViewBag.TargetDate = targetDate;

        return View(markingData.Data);
    }

    [HttpPost]
    public async Task<IActionResult> Mark(DevTrack.Domain.Features.Training.Models.BulkAttendanceRequest request)
    {
        var result = await _trainingService.MarkBulkAttendanceAsync(request);
        if (result.IsSuccess)
        {
            return RedirectToAction(nameof(Index), new { batchId = request.BatchId });
        }

        ModelState.AddModelError("", result.Message);
        return await Mark(request.BatchId, request.TrainingDate.ToString("yyyy-MM-dd"));
    }
}
