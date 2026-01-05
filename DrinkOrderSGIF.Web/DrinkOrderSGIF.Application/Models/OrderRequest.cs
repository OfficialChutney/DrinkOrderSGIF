namespace DrinkOrderSGIF.Application.Models;

public sealed record OrderItemRequest(Guid DrinkId, int Quantity);

public sealed record OrderRequest(Guid TeamId, IReadOnlyList<OrderItemRequest> Items);
