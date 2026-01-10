using System;
using System.IO;
using DrinkOrderSGIF.Application.Interfaces;
using DrinkOrderSGIF.Infrastructure.Data;
using DrinkOrderSGIF.Infrastructure.Services;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DrinkOrderSGIF.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        connectionString = Environment.ExpandEnvironmentVariables(connectionString);

        var builder = new SqliteConnectionStringBuilder(connectionString);
        if (!string.IsNullOrWhiteSpace(builder.DataSource) && builder.DataSource != ":memory:")
        {
            string? directory = Path.GetDirectoryName(builder.DataSource);
            if (!string.IsNullOrWhiteSpace(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }

        services.AddDbContext<AppDbContext>(options => options.UseSqlite(builder.ToString()));

        services.AddScoped<ITeamService, TeamService>();
        services.AddScoped<IDrinkService, DrinkService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IStatsService, StatsService>();
        services.AddScoped<DatabaseInitializer>();

        return services;
    }
}
