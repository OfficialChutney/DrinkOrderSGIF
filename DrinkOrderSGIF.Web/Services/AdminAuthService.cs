using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace DrinkOrderSGIF.Web.Services;

public sealed class AdminAuthService(ProtectedLocalStorage storage)
{
    private const string StorageKey = "DrinkOrderSGIF.Admin";
    private const string Password = "QOTSGIF2026";

    public async Task<bool> IsAuthenticatedAsync()
    {
        var result = await storage.GetAsync<bool>(StorageKey);
        return result.Success && result.Value;
    }

    public async Task<bool> TryLoginAsync(string password)
    {
        if (!string.Equals(password, Password, StringComparison.Ordinal))
        {
            return false;
        }

        await storage.SetAsync(StorageKey, true);
        return true;
    }

    public async Task LogoutAsync()
    {
        await storage.DeleteAsync(StorageKey);
    }
}
