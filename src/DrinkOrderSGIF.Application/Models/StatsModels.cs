namespace DrinkOrderSGIF.Application.Models;

public sealed record TeamSummary(Guid TeamId, string TeamName, int TotalDrinks, int OrderCount);

public sealed record TeamDrinkSummary(Guid DrinkId, string DrinkName, int Quantity);

public sealed record DrinkSummary(Guid DrinkId, string DrinkName, int Quantity);
