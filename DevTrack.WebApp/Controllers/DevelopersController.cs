using DevTrack.Domain.Features.Developers;
using DevTrack.Domain.Features.Developers.Models;
using Microsoft.AspNetCore.Mvc;

namespace DevTrack.WebApp.Controllers;

public class DevelopersController : Controller
{
    private readonly IDeveloperService _developerService;

    public DevelopersController(IDeveloperService developerService)
    {
        _developerService = developerService;
    }

    public async Task<IActionResult> Index()
    {
        var result = await _developerService.GetDevelopersAsync();
        return View(result.Data);
    }

    public IActionResult Create()
    {
        return View(new DeveloperRequest());
    }

    [HttpPost]
    public async Task<IActionResult> Create(DeveloperRequest request)
    {
        var result = await _developerService.CreateDeveloperAsync(request);
        if (result.IsSuccess)
        {
            return RedirectToAction(nameof(Index));
        }
        ModelState.AddModelError("", result.Message);
        return View(request);
    }
}
