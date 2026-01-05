using DrinkOrderSGIF.Application.Models;
using DrinkOrderSGIF.Domain.Entities;

namespace DrinkOrderSGIF.Application.Interfaces;

public interface IOrderService
{
    Task<IReadOnlyList<Order>> GetPendingOrdersAsync(CancellationToken cancellationToken = default);
    Task<Order> CreateOrderAsync(OrderRequest request, CancellationToken cancellationToken = default);
    Task CompleteOrderAsync(Guid orderId, CancellationToken cancellationToken = default);
}
