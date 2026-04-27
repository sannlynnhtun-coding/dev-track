using DevTrack.Domain.Features.Batches;
using DevTrack.Domain.Features.Batches.Models;
using DevTrack.Shared;
using Microsoft.AspNetCore.Mvc;

namespace DevTrack.WebApp.Controllers;

public class BatchesController : Controller
{
    private readonly IBatchService _batchService;

    public BatchesController(IBatchService batchService)
    {
        _batchService = batchService;
    }

    public async Task<IActionResult> Index(int pageAt = 1)
    {
        var result = await _batchService.GetBatchesAsync(new PaginationRequest { PageNumber = pageAt });
        return View(result);
    }

    public IActionResult Create()
    {
        return View(new BatchRequest { StartDate = DateOnly.FromDateTime(DateTime.Today), EndDate = DateOnly.FromDateTime(DateTime.Today.AddMonths(2)) });
    }

    [HttpPost]
    public async Task<IActionResult> Create(BatchRequest request)
    {
        var result = await _batchService.CreateBatchAsync(request);
        if (result.IsSuccess)
        {
            return RedirectToAction(nameof(Index));
        }
        ModelState.AddModelError("", result.Message);
        return View(request);
    }

    public async Task<IActionResult> ManageDevelopers(int id)
    {
        var batchResult = await _batchService.GetBatchByIdAsync(id);
        if (batchResult.IsFailure) return NotFound();

        var developersResult = await _batchService.GetBatchDevelopersAsync(id);
        ViewBag.Batch = batchResult.Data;

        return View(developersResult.Data);
    }

    [HttpPost]
    public async Task<IActionResult> ManageDevelopers(int id, List<int> selectedDeveloperIds)
    {
        var result = await _batchService.UpdateBatchAssignmentsAsync(id, selectedDeveloperIds);
        if (result.IsSuccess)
        {
            return RedirectToAction(nameof(Index));
        }
        
        ModelState.AddModelError("", result.Message);
        return await ManageDevelopers(id);
    }
}
