using System;
using EventPlanner.Api.Contracts.Taak;
using EventPlanner.Api.Persistence.Models;

namespace EventPlanner.Api.Services.Mapping;

public static class TaakMappingExtensions
{
    public static TaakResponseContract AsResponseContract(this Taak taak)
    {
        return new TaakResponseContract
        {
            Id = taak.Id,
            Naam = taak.Naam,
            Beschrijving = taak.Beschrijving,
            Belangrijkheid = taak.Belangrijkheid,
            Status = taak.Status,
            DeadlineDateTime = taak.DeadlineDateTime,
            EvenementId = taak.EvenementId,
            EvenementNaam = taak.Evenement.Naam
        };
    }

    public static Taak AsModel(this TaakRequestContract taakRequest, Evenement evenement, int id = 0)
    {
        return new Taak
        {
            Id = id,
            Naam = taakRequest.Naam,
            Beschrijving = taakRequest.Beschrijving,
            Belangrijkheid = taakRequest.Belangrijkheid,
            Status = taakRequest.Status,
            DeadlineDateTime = taakRequest.DeadlineDateTime,
            EvenementId = taakRequest.EvenementId,
            Evenement = evenement
        };
    }
}
