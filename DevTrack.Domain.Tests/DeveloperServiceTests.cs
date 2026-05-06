using DevTrack.Database.Entities;
using DevTrack.Domain.Features.Developers;
using DevTrack.Domain.Features.Developers.Models;
using DevTrack.Shared;

namespace DevTrack.Domain.Tests;

public class DeveloperServiceTests
{
    [Fact]
    public async Task CreateDeveloper_ShouldPersistAndReturnModel()
    {
        using var db = TestDbContextFactory.Create();
        var service = new DeveloperService(db);

        var result = await service.CreateDeveloperAsync(new DeveloperRequest
        {
            FullName = "Dev One",
            DeveloperCode = "DEV001",
            Email = "dev@local",
            IsActive = true
        });

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);
        Assert.Equal("Dev One", result.Data.FullName);
        Assert.Single(db.Developers);
    }

    [Fact]
    public async Task CreateDeveloper_ShouldFailWhenNameMissing()
    {
        using var db = TestDbContextFactory.Create();
        var service = new DeveloperService(db);

        var result = await service.CreateDeveloperAsync(new DeveloperRequest { FullName = "" });

        Assert.True(result.IsFailure);
        Assert.Equal("Full name is required.", result.Message);
    }

    [Fact]
    public async Task GetDevelopers_ShouldReturnPagedAlphabeticalList()
    {
        using var db = TestDbContextFactory.Create();
        db.Developers.AddRange(
            new Developer { Id = 1, FullName = "Zed", IsActive = true },
            new Developer { Id = 2, FullName = "Alpha", IsActive = true },
            new Developer { Id = 3, FullName = "Mike", IsActive = false });
        await db.SaveChangesAsync();

        var service = new DeveloperService(db);
        var result = await service.GetDevelopersAsync(new PaginationRequest { PageNumber = 1, PageSize = 2 });

        Assert.Equal(2, result.Data.Count);
        Assert.Equal("Alpha", result.Data[0].FullName);
        Assert.Equal("Mike", result.Data[1].FullName);
    }
}
