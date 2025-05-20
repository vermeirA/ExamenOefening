using System;
using EventPlanner.Shared;

namespace EventPlanner.Api.Contracts.Taak;

public class TaakResponseContract
{
    public int Id { get; set; }
    public string Naam { get; set; } = null!;
    public string? Beschrijving { get; set; }
    public BelangrijkheidEnum Belangrijkheid { get; set; }
    public StatusEnum Status { get; set; }
    public DateTime? DeadlineDateTime { get; set; }
    public int EvenementId { get; set; }
    public string EvenementNaam { get; set; } = null!;
}
