namespace DrinkOrderSGIF.Domain.Entities;

public sealed class OrderItem
{
    public Guid Id { get; set; }
    public Guid OrderId { get; set; }
    public Order? Order { get; set; }
    public Guid DrinkId { get; set; }
    public string DrinkName { get; set; } = string.Empty;
    public int Quantity { get; set; }
}
