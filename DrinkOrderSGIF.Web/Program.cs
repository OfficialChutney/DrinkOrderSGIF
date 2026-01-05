using DrinkOrderSGIF.Infrastructure.Data;
using DrinkOrderSGIF.Infrastructure.Extensions;
using DrinkOrderSGIF.Web.Components;
using DrinkOrderSGIF.Web.Hubs;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddSignalR();
builder.Services.AddInfrastructure();
builder.Services.AddScoped<DrinkOrderSGIF.Web.Services.OrderDraftState>();
builder.Services.AddScoped<DrinkOrderSGIF.Web.Services.AdminAuthService>();
builder.Services.AddScoped<ProtectedLocalStorage>();
builder.Services.AddScoped<DrinkOrderSGIF.Application.Interfaces.IOrderUpdateBroadcaster, DrinkOrderSGIF.Web.Services.SignalROrderUpdateBroadcaster>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();
app.MapHub<OrderHub>("/orderHub");

using (var scope = app.Services.CreateScope())
{
    var initializer = scope.ServiceProvider.GetRequiredService<DatabaseInitializer>();
    await initializer.InitializeAsync();
}

await app.RunAsync();
