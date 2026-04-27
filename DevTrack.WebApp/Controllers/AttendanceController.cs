using DevTrack.Domain.Features.Training;
using DevTrack.Domain.Features.Batches;
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

    public async Task<IActionResult> Index(int? batchId)
    {
        var batches = await _batchService.GetBatchesAsync();
        ViewBag.Batches = batches.Data;

        if (batchId.HasValue)
        {
            var summary = await _trainingService.GetAttendanceSummaryAsync(batchId.Value);
            ViewBag.SelectedBatchId = batchId.Value;
            return View(summary.Data);
        }

        return View(new List<DevTrack.Domain.Features.Training.Models.AttendanceSummaryResponse>());
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
