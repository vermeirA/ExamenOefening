using System;
using EventPlanner.Api.Contracts.Locatie;
using EventPlanner.Api.Persistence.Models;

namespace EventPlanner.Api.Services.Mapping;

public static class LocatieMappingExtensions
{
    public static LocatieResponseContract AsResponseContract(this Locatie locatie)
    {
        return new LocatieResponseContract
        {
            Id = locatie.Id,
            Naam = locatie.Naam,
            Beschrijving = locatie.Beschrijving,
            GpsLat = locatie.GpsLat,
            GpsLon = locatie.GpsLon
        };
    }

    public static Locatie AsModel(this LocatieRequestContract locatieRequest, int id = 0)
    {
        return new Locatie
        {
            Id = id,
            Naam = locatieRequest.Naam,
            Beschrijving = locatieRequest.Beschrijving,
            GpsLat = locatieRequest.GpsLat,
            GpsLon = locatieRequest.GpsLon
        };
    }
}
