using DrinkOrderSGIF.Application.Models;

namespace DrinkOrderSGIF.Application.Interfaces;

public interface IStatsService
{
    Task<IReadOnlyList<TeamSummary>> GetTeamSummariesAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<TeamDrinkSummary>> GetTeamDrinkSummaryAsync(Guid teamId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DrinkSummary>> GetDrinkTotalsAsync(CancellationToken cancellationToken = default);
}
