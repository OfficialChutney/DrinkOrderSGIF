using DrinkOrderSGIF.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DrinkOrderSGIF.Infrastructure.Data;

public sealed class DatabaseInitializer(AppDbContext dbContext)
{
    public async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        await dbContext.Database.EnsureCreatedAsync(cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
