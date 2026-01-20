using DrinkOrderSGIF.Domain.Entities;

namespace DrinkOrderSGIF.Web.Services;

public static class DrinkCurrencyExtensions
{
    public static string GetLabel(this DrinkCurrency currency)
    {
        return currency switch
        {
            DrinkCurrency.Dkk => "DKK",
            _ => "Klips"
        };
    }
}
