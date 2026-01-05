using DrinkOrderSGIF.Application.Interfaces;
using DrinkOrderSGIF.Infrastructure.Data;
using DrinkOrderSGIF.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DrinkOrderSGIF.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        var appData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        var dataDirectory = Path.Combine(appData, "DrinkOrderSGIF");
        Directory.CreateDirectory(dataDirectory);
        var dbPath = Path.Combine(dataDirectory, "drinkorders.db");

        services.AddDbContext<AppDbContext>(options => options.UseSqlite($"Data Source={dbPath}"));

        services.AddScoped<ITeamService, TeamService>();
        services.AddScoped<IDrinkService, DrinkService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IStatsService, StatsService>();
        services.AddScoped<DatabaseInitializer>();

        return services;
    }
}
