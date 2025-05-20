using System;
using MongoDB.Bson;

namespace EventPlanner.Shared;

public class AuditEntry
{
    public ObjectId Id { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public required string Onderwerp { get; set; }
    public required string Actie { get; set; }
    public string? OudeWaarde { get; set; }
    public string? NieuweWaarde { get; set; }
}
