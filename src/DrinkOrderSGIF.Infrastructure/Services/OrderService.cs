using DrinkOrderSGIF.Application.Interfaces;
using DrinkOrderSGIF.Application.Models;
using DrinkOrderSGIF.Domain.Entities;
using DrinkOrderSGIF.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DrinkOrderSGIF.Infrastructure.Services;

public sealed class OrderService(AppDbContext dbContext, IOrderUpdateBroadcaster? broadcaster) : IOrderService
{
    public async Task<IReadOnlyList<Order>> GetPendingOrdersAsync(CancellationToken cancellationToken = default)
    {
        var orders = await dbContext.Orders
            .AsNoTracking()
            .Include(order => order.Items)
            .Where(order => order.Status == OrderStatus.Pending)
            .ToListAsync(cancellationToken);

        return orders
            .OrderBy(order => order.CreatedAt)
            .ToList();
    }

    public async Task<Order> CreateOrderAsync(OrderRequest request, CancellationToken cancellationToken = default)
    {
        if (request.Items.Count == 0 || request.Items.All(item => item.Quantity <= 0))
        {
            throw new InvalidOperationException("Order must include at least one drink.");
        }

        var team = await dbContext.Teams.FirstOrDefaultAsync(t => t.Id == request.TeamId, cancellationToken);
        if (team is null || !team.IsActive)
        {
            throw new InvalidOperationException("Team is not available for ordering.");
        }

        var drinkIds = request.Items.Select(item => item.DrinkId).Distinct().ToList();
        var drinks = await dbContext.Drinks
            .Where(drink => drinkIds.Contains(drink.Id) && drink.IsActive)
            .ToListAsync(cancellationToken);

        var drinkLookup = drinks.ToDictionary(drink => drink.Id);

        var order = new Order
        {
            TeamId = team.Id,
            TeamName = team.Name,
            CreatedAt = DateTimeOffset.UtcNow,
            Status = OrderStatus.Pending,
            Items = new List<OrderItem>()
        };

        foreach (var item in request.Items.Where(i => i.Quantity > 0))
        {
            if (!drinkLookup.TryGetValue(item.DrinkId, out var drink))
            {
                continue;
            }

            order.Items.Add(new OrderItem
            {
                DrinkId = drink.Id,
                DrinkName = drink.Name,
                Quantity = item.Quantity
            });
        }

        if (order.Items.Count == 0)
        {
            throw new InvalidOperationException("Order must include at least one active drink.");
        }

        dbContext.Orders.Add(order);
        await dbContext.SaveChangesAsync(cancellationToken);
        if (broadcaster is not null)
        {
            await broadcaster.NotifyOrderCreatedAsync(order.Id, cancellationToken);
        }
        return order;
    }

    public async Task CompleteOrderAsync(Guid orderId, CancellationToken cancellationToken = default)
    {
        var order = await dbContext.Orders.FirstOrDefaultAsync(o => o.Id == orderId, cancellationToken);
        if (order is null)
        {
            return;
        }

        order.Status = OrderStatus.Completed;
        await dbContext.SaveChangesAsync(cancellationToken);
        if (broadcaster is not null)
        {
            await broadcaster.NotifyOrderCompletedAsync(order.Id, cancellationToken);
        }
    }
}
