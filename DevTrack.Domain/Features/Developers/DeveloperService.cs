using DevTrack.Database.Entities;
using DevTrack.Domain.Features.Developers.Models;
using DevTrack.Shared;
using Microsoft.EntityFrameworkCore;

namespace DevTrack.Domain.Features.Developers;

public class DeveloperService : IDeveloperService
{
    private readonly DevTrackDbContext _db;

    public DeveloperService(DevTrackDbContext db)
    {
        _db = db;
    }

    public async Task<Result<List<DeveloperResponse>>> GetDevelopersAsync()
    {
        var developers = await _db.Developers
            .Select(d => new DeveloperResponse
            {
                Id = d.Id,
                FullName = d.FullName,
                DeveloperCode = d.DeveloperCode,
                Email = d.Email,
                IsActive = d.IsActive ?? true
            })
            .ToListAsync();

        return Result<List<DeveloperResponse>>.Success(developers);
    }

    public async Task<Result<DeveloperResponse>> CreateDeveloperAsync(DeveloperRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.FullName))
            return Result<DeveloperResponse>.Failure("Full name is required.");

        var developer = new Developer
        {
            FullName = request.FullName,
            DeveloperCode = request.DeveloperCode,
            Email = request.Email,
            Phone = request.Phone,
            IsActive = request.IsActive,
            CreatedAt = DateTime.Now
        };

        _db.Developers.Add(developer);
        await _db.SaveChangesAsync();

        return Result<DeveloperResponse>.Success(new DeveloperResponse
        {
            Id = developer.Id,
            FullName = developer.FullName,
            DeveloperCode = developer.DeveloperCode,
            Email = developer.Email,
            IsActive = developer.IsActive ?? true
        }, "Developer created successfully.");
    }
}
