using System;
using System.Text.Json;
using EventPlanner.Api.Persistence;
using EventPlanner.Shared;

namespace EventPlanner.Api.Services;

public interface IAuditService
{
    Task LogEventAsync(string onderwerp, string actie, object? oudeWaarde, object? nieuweWaarde);
    Task<List<AuditEntry>> FilterAsync(string? onderwerp, string? actie);
}
public class AuditService(IAuditRepository auditRepository) : IAuditService
{
    public async Task<List<AuditEntry>> FilterAsync(string? onderwerp, string? actie)
    {
        return await auditRepository.FilterAsync(onderwerp, actie);
    }

    public async Task LogEventAsync(string onderwerp, string actie, object? oudeWaarde, object? nieuweWaarde)
    {
        if (onderwerp == null)
        {
            throw new ArgumentNullException(nameof(onderwerp));
        }
        if (actie == null)
        {
            throw new ArgumentNullException(nameof(actie));
        }
        await auditRepository.LogEventAsync(onderwerp, actie, JsonSerializer.Serialize(oudeWaarde), JsonSerializer.Serialize(nieuweWaarde));
    }
}
