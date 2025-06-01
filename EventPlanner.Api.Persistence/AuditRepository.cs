using System;
using EventPlanner.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace EventPlanner.Api.Persistence;

public interface IAuditRepository
{
    Task LogEventAsync(string onderwerp, string actie, string oudeWaarde, string nieuweWaarde);
    Task<List<AuditEntry>> FilterAsync(string? onderwerp, string? actie);
}

public class AuditRepository : IAuditRepository
{
    private readonly AuditDbContext _context;

    public AuditRepository(AuditDbContext context)
    {
        _context = context;
    }

    public async Task LogEventAsync(string onderwerp, string actie, string oudeWaarde, string nieuweWaarde)
    {
        var entry = new AuditEntry
        {
            Onderwerp = onderwerp,
            Actie = actie,
            OudeWaarde = oudeWaarde,
            NieuweWaarde = nieuweWaarde
        };

        await _context.AuditLogs.AddAsync(entry);
        await _context.SaveChangesAsync();
    }

    public async Task<List<AuditEntry>> FilterAsync(string? onderwerp, string? actie)
    {
        IQueryable<AuditEntry> query = _context.AuditLogs;

        if (!string.IsNullOrEmpty(onderwerp))
            query = query.Where(a => a.Onderwerp == onderwerp);

        if (!string.IsNullOrEmpty(actie))
            query = query.Where(a => a.Actie == actie);

        return await query.ToListAsync();
    }
}
