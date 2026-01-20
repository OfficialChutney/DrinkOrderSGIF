namespace DrinkOrderSGIF.Domain.Entities;

public sealed class Drink
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Price { get; set; }
    public int UnitsPerPrice { get; set; } = 1;
    public DrinkCurrency Currency { get; set; } = DrinkCurrency.Klips;
    public bool IsActive { get; set; } = true;

    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
