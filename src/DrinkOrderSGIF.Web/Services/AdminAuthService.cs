using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace DrinkOrderSGIF.Web.Services;

public sealed class AdminAuthService(ProtectedLocalStorage storage)
{
    private const string StorageKey = "DrinkOrderSGIF.Admin";
    private const string Password = "QOTSGIF2026";

    public event Action<bool>? AuthStateChanged;

    public async Task<bool> IsAuthenticatedAsync()
    {
        try
        {
            var result = await storage.GetAsync<bool>(StorageKey);
            return result.Success && result.Value;
        }
        catch (System.Security.Cryptography.CryptographicException)
        {
            // Stored payload is invalid (e.g., key change). Clear it to recover.
            await storage.DeleteAsync(StorageKey);
            AuthStateChanged?.Invoke(false);
            return false;
        }
    }

    public async Task<bool> TryLoginAsync(string password)
    {
        if (!string.Equals(password, Password, StringComparison.Ordinal))
        {
            return false;
        }

        await storage.SetAsync(StorageKey, true);
        AuthStateChanged?.Invoke(true);
        return true;
    }

    public async Task LogoutAsync()
    {
        await storage.DeleteAsync(StorageKey);
        AuthStateChanged?.Invoke(false);
    }
}
