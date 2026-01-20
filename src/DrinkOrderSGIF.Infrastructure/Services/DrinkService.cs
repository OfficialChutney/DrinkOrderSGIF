using DrinkOrderSGIF.Application.Interfaces;
using DrinkOrderSGIF.Domain.Entities;
using DrinkOrderSGIF.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DrinkOrderSGIF.Infrastructure.Services;

public sealed class DrinkService(AppDbContext dbContext) : IDrinkService
{
    public async Task<IReadOnlyList<Drink>> GetActiveDrinksAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.Drinks
            .AsNoTracking()
            .Where(drink => drink.IsActive)
            .OrderBy(drink => drink.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Drink>> GetAllDrinksAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.Drinks
            .AsNoTracking()
            .OrderBy(drink => drink.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<Drink> AddDrinkAsync(string name, int price, int unitsPerPrice, decimal? klipsPrice, CancellationToken cancellationToken = default)
    {
        var trimmed = name.Trim();
        if (string.IsNullOrWhiteSpace(trimmed))
        {
            throw new ArgumentException("Drink name is required.", nameof(name));
        }

        if (price < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(price), "Price must be zero or higher.");
        }

        if (klipsPrice < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(klipsPrice), "Klips price must be zero or higher.");
        }

        if (unitsPerPrice < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(unitsPerPrice), "Units per price must be at least 1.");
        }

        var existing = await dbContext.Drinks
            .FirstOrDefaultAsync(drink => drink.Name.ToLower() == trimmed.ToLower(), cancellationToken);

        if (existing is not null)
        {
            existing.IsActive = true;
            existing.Price = price;
            existing.UnitsPerPrice = unitsPerPrice;
            existing.KlipsPrice = klipsPrice;
            await dbContext.SaveChangesAsync(cancellationToken);
            return existing;
        }

        var drink = new Drink
        {
            Name = trimmed,
            Price = price,
            UnitsPerPrice = unitsPerPrice,
            KlipsPrice = klipsPrice,
            IsActive = true
        };
        dbContext.Drinks.Add(drink);
        await dbContext.SaveChangesAsync(cancellationToken);
        return drink;
    }

    public async Task UpdateDrinkAsync(Guid drinkId, string name, int price, int unitsPerPrice, decimal? klipsPrice, CancellationToken cancellationToken = default)
    {
        var trimmed = name.Trim();
        if (string.IsNullOrWhiteSpace(trimmed))
        {
            throw new ArgumentException("Drink name is required.", nameof(name));
        }

        if (price < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(price), "Price must be zero or higher.");
        }

        if (klipsPrice < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(klipsPrice), "Klips price must be zero or higher.");
        }

        if (unitsPerPrice < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(unitsPerPrice), "Units per price must be at least 1.");
        }

        var drink = await dbContext.Drinks.FirstOrDefaultAsync(d => d.Id == drinkId, cancellationToken);
        if (drink is null)
        {
            return;
        }

        drink.Name = trimmed;
        drink.Price = price;
        drink.UnitsPerPrice = unitsPerPrice;
        drink.KlipsPrice = klipsPrice;
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task SetDrinkActiveAsync(Guid drinkId, bool isActive, CancellationToken cancellationToken = default)
    {
        var drink = await dbContext.Drinks.FirstOrDefaultAsync(d => d.Id == drinkId, cancellationToken);
        if (drink is null)
        {
            return;
        }

        drink.IsActive = isActive;
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
