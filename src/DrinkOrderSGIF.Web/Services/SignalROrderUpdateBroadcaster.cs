using DrinkOrderSGIF.Application.Interfaces;
using DrinkOrderSGIF.Web.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace DrinkOrderSGIF.Web.Services;

public sealed class SignalROrderUpdateBroadcaster(IHubContext<OrderHub> hubContext) : IOrderUpdateBroadcaster
{
    public Task NotifyOrderCreatedAsync(Guid orderId, CancellationToken cancellationToken = default)
    {
        return hubContext.Clients.All.SendAsync("orders-changed", orderId, cancellationToken);
    }

    public Task NotifyOrderCompletedAsync(Guid orderId, CancellationToken cancellationToken = default)
    {
        return hubContext.Clients.All.SendAsync("orders-changed", orderId, cancellationToken);
    }
}
