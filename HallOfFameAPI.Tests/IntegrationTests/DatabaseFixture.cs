using HallOfFameAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace HallOfFameAPI.Tests.IntegrationTests;

public class DatabaseFixture : IDisposable
{
    public HallOfFameDbContext DbContext { get; }

    public DatabaseFixture()
    {
        var options = new DbContextOptionsBuilder<HallOfFameDbContext>()
            .UseNpgsql("Host=localhost;Database=hall_of_fame;Username=postgres;Password=QWEasd123")
            .Options;
            
        DbContext = new HallOfFameDbContext(options);
        DbContext.Database.EnsureCreated();
    }

    public void Dispose()
    {
        DbContext.Database.EnsureDeleted();
        DbContext.Dispose();
    }
}