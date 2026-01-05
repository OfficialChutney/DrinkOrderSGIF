using DrinkOrderSGIF.Web.Services;
using Microsoft.AspNetCore.Components;

namespace DrinkOrderSGIF.Web.Components.Pages;

public abstract class AdminPageBase : ComponentBase, IDisposable
{
    [Inject] protected AdminAuthService AuthService { get; set; } = default!;
    [Inject] protected NavigationManager Navigation { get; set; } = default!;

    protected bool IsAuthorized { get; set; }

    private bool _authChecked;
    private bool _isDisposed;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await EnsureAuthorizedAsync();
        }
    }

    protected async Task<bool> EnsureAuthorizedAsync()
    {
        if (!_authChecked)
        {
            _authChecked = true;
            IsAuthorized = await AuthService.IsAuthenticatedAsync();
        }

        if (!IsAuthorized)
        {
            Navigation.NavigateTo("/admin/login");
            return false;
        }

        if (!_isDisposed)
        {
            await InvokeAsync(StateHasChanged);
        }

        return true;
    }

    public void Dispose()
    {
        _isDisposed = true;
    }
}
