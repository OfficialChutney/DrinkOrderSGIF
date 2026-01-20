using DrinkOrderSGIF.Domain.Entities;

namespace DrinkOrderSGIF.Application.Interfaces;

public interface IDrinkService
{
    Task<IReadOnlyList<Drink>> GetActiveDrinksAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Drink>> GetAllDrinksAsync(CancellationToken cancellationToken = default);
    Task<Drink> AddDrinkAsync(string name, int price, decimal? klipsPrice, CancellationToken cancellationToken = default);
    Task UpdateDrinkAsync(Guid drinkId, string name, int price, decimal? klipsPrice, CancellationToken cancellationToken = default);
    Task SetDrinkActiveAsync(Guid drinkId, bool isActive, CancellationToken cancellationToken = default);
}
