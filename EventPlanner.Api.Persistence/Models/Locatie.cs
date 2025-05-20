using System;
using System.ComponentModel.DataAnnotations;

namespace EventPlanner.Api.Persistence.Models;

public class Locatie
{
    public int Id { get; set; }
    [MaxLength(100)]
    public string Naam { get; set; } = null!;
    [MaxLength(300)]
    public string? Beschrijving { get; set; }
    [Range(-90, 90)]
    public double GpsLat { get; set; }
    [Range(-180, 180)]
    public double GpsLon { get; set; }
    public ICollection<Evenement> Evenementen { get; set; } = new List<Evenement>();
}
