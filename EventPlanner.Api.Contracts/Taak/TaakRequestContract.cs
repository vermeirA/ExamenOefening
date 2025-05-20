using System;
using EventPlanner.Shared;

namespace EventPlanner.Api.Contracts.Taak;

public class TaakRequestContract
{
    public int EvenementId { get; set; }
    public string Naam { get; set; } = null!;
    public string? Beschrijving { get; set; }
    public BelangrijkheidEnum Belangrijkheid { get; set; }
    public StatusEnum Status { get; set; } = StatusEnum.Todo;
    public DateTime? DeadlineDateTime { get; set; }
}
