using DrinkOrderSGIF.Application.Interfaces;
using DrinkOrderSGIF.Application.Models;
using DrinkOrderSGIF.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DrinkOrderSGIF.Infrastructure.Services;

public sealed class StatsService(AppDbContext dbContext) : IStatsService
{
    public async Task<IReadOnlyList<TeamSummary>> GetTeamSummariesAsync(CancellationToken cancellationToken = default)
    {
        var orders = await dbContext.Orders
            .AsNoTracking()
            .Include(order => order.Items)
            .ToListAsync(cancellationToken);

        return orders
            .GroupBy(order => new { order.TeamId, order.TeamName })
            .Select(group => new TeamSummary(
                group.Key.TeamId,
                group.Key.TeamName,
                group.SelectMany(order => order.Items).Sum(item => item.Quantity),
                group.Count()))
            .OrderBy(summary => summary.TeamName)
            .ToList();
    }

    public async Task<IReadOnlyList<TeamDrinkSummary>> GetTeamDrinkSummaryAsync(Guid teamId, CancellationToken cancellationToken = default)
    {
        var items = await dbContext.Orders
            .AsNoTracking()
            .Where(order => order.TeamId == teamId)
            .SelectMany(order => order.Items)
            .ToListAsync(cancellationToken);

        return items
            .GroupBy(item => new { item.DrinkId, item.DrinkName })
            .Select(group => new TeamDrinkSummary(
                group.Key.DrinkId,
                group.Key.DrinkName,
                group.Sum(item => item.Quantity)))
            .OrderBy(summary => summary.DrinkName)
            .ToList();
    }

    public async Task<IReadOnlyList<DrinkSummary>> GetDrinkTotalsAsync(CancellationToken cancellationToken = default)
    {
        var items = await dbContext.OrderItems
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return items
            .GroupBy(item => new { item.DrinkId, item.DrinkName })
            .Select(group => new DrinkSummary(
                group.Key.DrinkId,
                group.Key.DrinkName,
                group.Sum(item => item.Quantity)))
            .OrderBy(summary => summary.DrinkName)
            .ToList();
    }
}
