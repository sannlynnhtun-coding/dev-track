using DevTrack.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace DevTrack.Domain.Tests;

internal static class TestDbContextFactory
{
    public static DevTrackDbContext Create()
    {
        var options = new DbContextOptionsBuilder<DevTrackDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new TestDevTrackDbContext(options);
    }

    private sealed class TestDevTrackDbContext : DevTrackDbContext
    {
        public TestDevTrackDbContext(DbContextOptions<DevTrackDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());
            }
        }
    }
}
