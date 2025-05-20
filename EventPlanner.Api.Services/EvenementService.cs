using System;
using System.Text.Json;
using EventPlanner.Api.Contracts.Evenement;
using EventPlanner.Api.Persistence;
using EventPlanner.Api.Services.Mapping;
using EventPlanner.Shared;

namespace EventPlanner.Api.Services;

public interface IEvenementService
{
    Task<EvenementResponseContract?> GetByIdAsync(int id);
    Task<List<EvenementResponseContract>> GetAllAsync();
    Task<EvenementResponseContract> AddAsync(EvenementRequestContract evenement);
    Task<EvenementResponseContract> UpdateAsync(int id, EvenementRequestContract evenement);
    Task DeleteAsync(int id);
    Task<OverzichtRapportDto> GetOverzichtRapportAsync(int id);
}
public class EvenementService(IEvenementRepository evenementRepository, ILocatieRepository locatieRepository, IAuditService auditService) : IEvenementService
{
    public async Task<EvenementResponseContract> AddAsync(EvenementRequestContract evenement)
    {
        //fetch locatie
        var locatie = await locatieRepository.GetByIdAsync(evenement.LocatieId);
        if (locatie == null)
        {
            throw new ArgumentException($"Locatie met id {evenement.LocatieId} bestaat niet.");
        }
        //create evenement
        var addedEvenement = (await evenementRepository.AddAsync(evenement.AsEvenementModel(locatie))).AsResponseContract();
        //create audit log
        await auditService.LogEventAsync("Evenement", "C", null, addedEvenement);
        return addedEvenement;
    }

    public async Task DeleteAsync(int id)
    {
        var evenementToDelete = await evenementRepository.GetByIdAsync(id);
        if (evenementToDelete == null)
        {
            throw new ArgumentException($"Evenement met id {id} bestaat niet.");
        }
        //create audit log
        await auditService.LogEventAsync("Evenement", "D", evenementToDelete.AsResponseContract(), null);
        await evenementRepository.DeleteAsync(id);
    }

    public async Task<List<EvenementResponseContract>> GetAllAsync()
    {
        var evenement = await evenementRepository.GetAllAsync();
        var evenementList = evenement.Select(l => l.AsResponseContract()).ToList();
        //create audit log
        await auditService.LogEventAsync("Evenement", "R", null, evenementList);
        return evenementList;
    }

    public async Task<EvenementResponseContract?> GetByIdAsync(int id)
    {
        var evenement = await evenementRepository.GetByIdAsync(id);
        var response = evenement?.AsResponseContract();
        //create audit log
        await auditService.LogEventAsync("Evenement", "R", null, response);
        return response;
    }

    public async Task<EvenementResponseContract> UpdateAsync(int id, EvenementRequestContract evenement)
    {
        var evenementToUpdate = await evenementRepository.GetByIdAsync(id);
        if (evenementToUpdate == null) throw new ArgumentException($"Evenement met id {id} bestaat niet.");

        var locatie = await locatieRepository.GetByIdAsync(evenement.LocatieId);
        if (locatie == null) throw new ArgumentException($"Locatie met id {evenement.LocatieId} bestaat niet.");

        var oudeWaarde = evenementToUpdate.AsResponseContract();
        var nieuweWaarde = (await evenementRepository.UpdateAsync(evenement.AsEvenementModel(locatie, id))).AsResponseContract();
        await auditService.LogEventAsync("Evenement", "U", oudeWaarde, nieuweWaarde);
        return nieuweWaarde;
    }
    public async Task<OverzichtRapportDto> GetOverzichtRapportAsync(int id)
    {
        return await evenementRepository.GetOverzichtRapportAsync(id);
    }
}


