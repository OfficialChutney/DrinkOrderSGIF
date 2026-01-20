namespace DrinkOrderSGIF.Web.Services;

public sealed class OrderDraftState
{
    private readonly Dictionary<Guid, DraftItem> _items = new();

    public Guid? TeamId { get; private set; }
    public string TeamName { get; private set; } = string.Empty;

    public IReadOnlyList<DraftItem> Items => _items.Values.OrderBy(item => item.DrinkName).ToList();

    public void SetTeam(Guid teamId, string teamName)
    {
        if (TeamId == teamId)
        {
            return;
        }

        TeamId = teamId;
        TeamName = teamName;
        _items.Clear();
    }

    public int GetQuantity(Guid drinkId)
    {
        return _items.TryGetValue(drinkId, out var item) ? item.TotalUnits : 0;
    }

    public int GetPrice(Guid drinkId)
    {
        return _items.TryGetValue(drinkId, out var item) ? item.Price : 0;
    }

    public int TotalPrice => _items.Values.Sum(item => item.TotalPriceDkk);

    public decimal TotalKlips => _items.Values
        .Where(item => item.KlipsPrice.HasValue)
        .Sum(item => item.TotalPriceKlips);

    public int MandatoryDkk => _items.Values
        .Where(item => !item.KlipsPrice.HasValue)
        .Sum(item => item.TotalPriceDkk);

    public void Increment(Guid drinkId, string drinkName, int price, decimal? klipsPrice)
    {
        if (_items.TryGetValue(drinkId, out var item))
        {
            item.Quantity += 1;
            item.Price = price;
            item.KlipsPrice = klipsPrice;
        }
        else
        {
            _items[drinkId] = new DraftItem(drinkId, drinkName, price, klipsPrice, 1);
        }
    }

    public void Decrement(Guid drinkId)
    {
        if (!_items.TryGetValue(drinkId, out var item))
        {
            return;
        }

        item.Quantity -= 1;
        if (item.Quantity <= 0)
        {
            _items.Remove(drinkId);
        }
    }

    public void Clear()
    {
        TeamId = null;
        TeamName = string.Empty;
        _items.Clear();
    }
}

public sealed class DraftItem
{
    public DraftItem(Guid drinkId, string drinkName, int price, decimal? klipsPrice, int quantity)
    {
        DrinkId = drinkId;
        DrinkName = drinkName;
        Price = price;
        KlipsPrice = klipsPrice;
        Quantity = quantity;
    }

    public Guid DrinkId { get; }
    public string DrinkName { get; }
    public int Price { get; set; }
    public decimal? KlipsPrice { get; set; }
    public int Quantity { get; set; }
    public int TotalUnits => Quantity;
    public int TotalPriceDkk => Quantity * Price;
    public decimal TotalPriceKlips => KlipsPrice.HasValue ? Quantity * KlipsPrice.Value : 0;
}
