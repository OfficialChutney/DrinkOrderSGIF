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

    public int GetIncrementCount(Guid drinkId)
    {
        return _items.TryGetValue(drinkId, out var item) ? item.IncrementCount : 0;
    }

    public int GetPrice(Guid drinkId)
    {
        return _items.TryGetValue(drinkId, out var item) ? item.Price : 0;
    }

    public int TotalPrice => _items.Values.Sum(item => item.TotalPrice);

    public void Increment(Guid drinkId, string drinkName, int price, int unitsPerPrice)
    {
        if (_items.TryGetValue(drinkId, out var item))
        {
            item.IncrementCount += 1;
            item.Price = price;
            item.UnitsPerPrice = unitsPerPrice;
        }
        else
        {
            _items[drinkId] = new DraftItem(drinkId, drinkName, price, unitsPerPrice, 1);
        }
    }

    public void Decrement(Guid drinkId)
    {
        if (!_items.TryGetValue(drinkId, out var item))
        {
            return;
        }

        item.IncrementCount -= 1;
        if (item.IncrementCount <= 0)
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
    public DraftItem(Guid drinkId, string drinkName, int price, int unitsPerPrice, int incrementCount)
    {
        DrinkId = drinkId;
        DrinkName = drinkName;
        Price = price;
        UnitsPerPrice = unitsPerPrice;
        IncrementCount = incrementCount;
    }

    public Guid DrinkId { get; }
    public string DrinkName { get; }
    public int Price { get; set; }
    public int UnitsPerPrice { get; set; }
    public int IncrementCount { get; set; }
    public int TotalUnits => IncrementCount * UnitsPerPrice;
    public int TotalPrice => IncrementCount * Price;
}
