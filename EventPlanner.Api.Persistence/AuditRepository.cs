using System;
using EventPlanner.Shared;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace EventPlanner.Api.Persistence;

public interface IAuditRepository
{
    Task LogEventAsync(string onderwerp, string actie, string oudeWaarde, string nieuweWaarde);
    Task<List<AuditEntry>> FilterAsync(string? onderwerp, string? actie);
}

public class AuditRepository : IAuditRepository
{
    private readonly IMongoCollection<AuditEntry> _collection;

    public AuditRepository(IConfiguration config)
    {
        var client = new MongoClient(config.GetConnectionString("MongoDb"));
        var db = client.GetDatabase("eventplanner_audit");
        _collection = db.GetCollection<AuditEntry>("auditlog");
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
        await _collection.InsertOneAsync(entry);
    }

    public async Task<List<AuditEntry>> FilterAsync(string? onderwerp, string? actie)
    {
        var filter = Builders<AuditEntry>.Filter.Empty;

        if (!string.IsNullOrEmpty(onderwerp))
            filter &= Builders<AuditEntry>.Filter.Eq(a => a.Onderwerp, onderwerp);

        if (!string.IsNullOrEmpty(actie))
            filter &= Builders<AuditEntry>.Filter.Eq(a => a.Actie, actie);

        return await _collection.Find(filter).ToListAsync();
    }
}
