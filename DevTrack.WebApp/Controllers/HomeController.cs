using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using DevTrack.Domain.Features.Dashboard;
using DevTrack.Shared;
using DevTrack.WebApp.Models;

namespace DevTrack.WebApp.Controllers;

public class HomeController : Controller
{
    private readonly IDashboardService _dashboardService;

    public HomeController(IDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    public async Task<IActionResult> Index()
    {
        var result = await _dashboardService.GetDashboardDataAsync();
        return View(result.Data);
    }

    public async Task<IActionResult> Download()
    {
        var result = await _dashboardService.GetDashboardReportAsync();
        if (!result.IsSuccess || result.Data == null)
        {
            return BadRequest("Could not generate report.");
        }

        var builder = new System.Text.StringBuilder();
        builder.AppendLine("Batch Name,Mentor,Developers");

        foreach (var item in result.Data)
        {
            builder.AppendLine($"{item.BatchName},{item.MentorName},{item.DeveloperCount}");
        }

        var fileName = $"Dashboard_Report_{DateTime.Now:yyyyMMdd}.csv";
        return File(System.Text.Encoding.UTF8.GetBytes(builder.ToString()), "text/csv", fileName);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
