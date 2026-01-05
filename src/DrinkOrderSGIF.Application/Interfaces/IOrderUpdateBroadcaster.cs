namespace DrinkOrderSGIF.Application.Interfaces;

public interface IOrderUpdateBroadcaster
{
    Task NotifyOrderCreatedAsync(Guid orderId, CancellationToken cancellationToken = default);
    Task NotifyOrderCompletedAsync(Guid orderId, CancellationToken cancellationToken = default);
}
