using System;

namespace EventPlanner.Api.Contracts.Evenement;

public class EvenementRequestContract
{
    public int LocatieId { get; set; }
    public string Naam { get; set; } = null!;
    public DateTime StartDateTime { get; set; }
    public DateTime EindDateTime { get; set; }
}
