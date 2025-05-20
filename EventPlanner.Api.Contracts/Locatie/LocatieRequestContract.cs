using System;

namespace EventPlanner.Api.Contracts.Locatie;

public class LocatieRequestContract
{
    public string Naam { get; set; } = null!;
    public string? Beschrijving { get; set; }
    public double GpsLat { get; set; }
    public double GpsLon { get; set; }
}
