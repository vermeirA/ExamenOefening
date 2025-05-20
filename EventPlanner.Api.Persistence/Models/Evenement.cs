using System;
using System.ComponentModel.DataAnnotations;

namespace EventPlanner.Api.Persistence.Models;

public class Evenement
{
    public int Id { get; set; }
    [MaxLength(100)]
    public string Naam { get; set; } = null!;
    public DateTime StartDateTime { get; set; }
    public DateTime EindDateTime { get; set; }
    public Locatie Locatie { get; set; } = null!;
    public ICollection<Taak> Taken { get; set; } = new List<Taak>();
    public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
}
