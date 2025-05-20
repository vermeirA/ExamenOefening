using System;
using System.ComponentModel.DataAnnotations;
using EventPlanner.Shared;

namespace EventPlanner.Api.Persistence.Models;

public class Taak
{
    public int Id { get; set; }
    public int EvenementId { get; set; }
    [MaxLength(100)]
    public string Naam { get; set; } = null!;
    [MaxLength(300)]
    public string? Beschrijving { get; set; }
    public BelangrijkheidEnum Belangrijkheid { get; set; } = BelangrijkheidEnum.Must;
    public StatusEnum Status { get; set; } = StatusEnum.Todo;
    public DateTime? DeadlineDateTime { get; set; }
    public Evenement Evenement { get; set; } = null!;
    public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
}
