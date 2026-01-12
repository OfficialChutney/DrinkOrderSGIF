using System;
using DrinkOrderSGIF.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;

namespace DrinkOrderSGIF.Infrastructure.Data;

public sealed class DatabaseInitializer(AppDbContext dbContext)
{
    public async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        await dbContext.Database.EnsureCreatedAsync(cancellationToken);
        try
        {
            await dbContext.Teams.AsNoTracking().AnyAsync(cancellationToken);
        }
        catch (SqliteException ex) when (ex.SqliteErrorCode == 1
            && ex.Message.Contains("no such table", StringComparison.OrdinalIgnoreCase))
        {
            await dbContext.Database.EnsureDeletedAsync(cancellationToken);
            await dbContext.Database.EnsureCreatedAsync(cancellationToken);
        }
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
