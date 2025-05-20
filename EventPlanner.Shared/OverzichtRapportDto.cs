using System;

namespace EventPlanner.Shared;

public class OverzichtRapportDto
{
    public int EvenementId { get; set; }
    public required string EvenementNaam { get; set; }
    public int LocatieId { get; set; }
    public required string LocatieNaam { get; set; }
    public double PercentageTakenVoltooid { get; set; }
    public int AantalMustTakenInTodo { get; set; }
    public DateTime LaatsteUpdate { get; set; }
}
