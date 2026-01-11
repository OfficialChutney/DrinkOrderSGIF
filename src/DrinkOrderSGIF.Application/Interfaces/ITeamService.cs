using DrinkOrderSGIF.Domain.Entities;

namespace DrinkOrderSGIF.Application.Interfaces;

public interface ITeamService
{
    Task<IReadOnlyList<Team>> GetActiveTeamsAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Team>> GetAllTeamsAsync(CancellationToken cancellationToken = default);
    Task<Team?> GetTeamByIdAsync(Guid teamId, CancellationToken cancellationToken = default);
    Task<Team> AddTeamAsync(string name, CancellationToken cancellationToken = default);
    Task UpdateTeamAsync(Guid teamId, string name, CancellationToken cancellationToken = default);
    Task SetTeamActiveAsync(Guid teamId, bool isActive, CancellationToken cancellationToken = default);
}
