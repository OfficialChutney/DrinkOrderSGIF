using DrinkOrderSGIF.Application.Interfaces;
using DrinkOrderSGIF.Domain.Entities;
using DrinkOrderSGIF.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DrinkOrderSGIF.Infrastructure.Services;

public sealed class TeamService(AppDbContext dbContext) : ITeamService
{
    public async Task<IReadOnlyList<Team>> GetActiveTeamsAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.Teams
            .AsNoTracking()
            .Where(team => team.IsActive)
            .OrderBy(team => team.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Team>> GetAllTeamsAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.Teams
            .AsNoTracking()
            .OrderBy(team => team.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<Team?> GetTeamByIdAsync(Guid teamId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Teams
            .AsNoTracking()
            .FirstOrDefaultAsync(team => team.Id == teamId, cancellationToken);
    }

    public async Task<Team> AddTeamAsync(string name, CancellationToken cancellationToken = default)
    {
        var trimmed = name.Trim();
        if (string.IsNullOrWhiteSpace(trimmed))
        {
            throw new ArgumentException("Team name is required.", nameof(name));
        }

        var existing = await dbContext.Teams
            .FirstOrDefaultAsync(team => team.Name.ToLower() == trimmed.ToLower(), cancellationToken);

        if (existing is not null)
        {
            existing.IsActive = true;
            await dbContext.SaveChangesAsync(cancellationToken);
            return existing;
        }

        var team = new Team { Name = trimmed, IsActive = true };
        dbContext.Teams.Add(team);
        await dbContext.SaveChangesAsync(cancellationToken);
        return team;
    }

    public async Task UpdateTeamAsync(Guid teamId, string name, CancellationToken cancellationToken = default)
    {
        var trimmed = name.Trim();
        if (string.IsNullOrWhiteSpace(trimmed))
        {
            throw new ArgumentException("Team name is required.", nameof(name));
        }

        var team = await dbContext.Teams.FirstOrDefaultAsync(t => t.Id == teamId, cancellationToken);
        if (team is null)
        {
            return;
        }

        team.Name = trimmed;
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task SetTeamActiveAsync(Guid teamId, bool isActive, CancellationToken cancellationToken = default)
    {
        var team = await dbContext.Teams.FirstOrDefaultAsync(t => t.Id == teamId, cancellationToken);
        if (team is null)
        {
            return;
        }

        team.IsActive = isActive;
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
