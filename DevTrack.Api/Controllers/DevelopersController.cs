using DevTrack.Domain.Features.Developers;
using DevTrack.Domain.Features.Developers.Models;
using DevTrack.Shared;
using Microsoft.AspNetCore.Mvc;

namespace DevTrack.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DevelopersController : ControllerBase
{
    private readonly IDeveloperService _developerService;

    public DevelopersController(IDeveloperService developerService)
    {
        _developerService = developerService;
    }

    [HttpGet]
    public async Task<IActionResult> GetDevelopers([FromQuery] PaginationRequest request)
    {
        var result = await _developerService.GetDevelopersAsync(request);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetDeveloper(int id)
    {
        var result = await _developerService.GetDeveloperByIdAsync(id);
        return result.IsSuccess ? Ok(result) : NotFound(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateDeveloper(DeveloperRequest request)
    {
        var result = await _developerService.CreateDeveloperAsync(request);
        return Ok(result);
    }
}
