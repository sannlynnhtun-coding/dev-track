using DevTrack.Database.Entities;
using DevTrack.Shared;
using Microsoft.EntityFrameworkCore;

namespace DevTrack.Domain.Features.Developers.Models;

public class DeveloperRequest
{
    public string FullName { get; set; } = string.Empty;
    public string? DeveloperCode { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public bool IsActive { get; set; } = true;
}

public class DeveloperResponse
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string? DeveloperCode { get; set; }
    public string? Email { get; set; }
    public bool IsActive { get; set; }
}
