using System;
using EventPlanner.Api.Contracts.Evenement;
using EventPlanner.Api.Persistence.Models;

namespace EventPlanner.Api.Services.Mapping;

public static class EvenementMappingExtensions
{
    public static EvenementResponseContract AsResponseContract(this Evenement evenement)
    {
        return new EvenementResponseContract
        {
            Id = evenement.Id,
            Naam = evenement.Naam,
            StartDateTime = evenement.StartDateTime,
            EindDateTime = evenement.EindDateTime,
            LocatieId = evenement.Locatie.Id,
            LocatieNaam = evenement.Locatie.Naam
        };
    }

    public static Evenement AsEvenementModel(this EvenementRequestContract evenementRequest, Locatie locatie, int id = 0)
    {
        return new Evenement
        {
            Id = id,
            Naam = evenementRequest.Naam,
            StartDateTime = evenementRequest.StartDateTime,
            EindDateTime = evenementRequest.EindDateTime,
            Locatie = locatie
        };
    }
}
