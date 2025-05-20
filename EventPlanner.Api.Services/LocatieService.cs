using System;
using EventPlanner.Api.Contracts.Locatie;
using EventPlanner.Api.Persistence;
using EventPlanner.Api.Services.Mapping;

namespace EventPlanner.Api.Services;

public interface ILocatieService
{
    Task<LocatieResponseContract?> GetByIdAsync(int id);
    Task<List<LocatieResponseContract>> GetAllAsync();
    Task<LocatieResponseContract> AddAsync(LocatieRequestContract locatie);
    Task<LocatieResponseContract> UpdateAsync(int id, LocatieRequestContract locatie);
    Task DeleteAsync(int id);
}
public class LocatieService(ILocatieRepository locatieRepository, IAuditService auditService) : ILocatieService
{
    public async Task<LocatieResponseContract> AddAsync(LocatieRequestContract locatie)
    {
        var addedLocation = (await locatieRepository.AddAsync(locatie.AsModel())).AsResponseContract();
        await auditService.LogEventAsync("Locatie", "C", null, addedLocation);
        return addedLocation;
    }

    public async Task DeleteAsync(int id)
    {
        var locatieToDelete = await locatieRepository.GetByIdAsync(id);
        if (locatieToDelete == null)
        {
            throw new ArgumentException($"Locatie met id {id} bestaat niet.");
        }
        //create audit log
        await auditService.LogEventAsync("Locatie", "D", locatieToDelete.AsResponseContract(), null);
        await locatieRepository.DeleteAsync(id);
    }

    public async Task<List<LocatieResponseContract>> GetAllAsync()
    {
        var locaties = await locatieRepository.GetAllAsync();
        var locatieList = locaties.Select(l => l.AsResponseContract()).ToList();
        //create audit log
        await auditService.LogEventAsync("Locatie", "R", null, locatieList);
        return locaties.Select(l => l.AsResponseContract()).ToList();
    }

    public async Task<LocatieResponseContract?> GetByIdAsync(int id)
    {
        var locatie = await locatieRepository.GetByIdAsync(id);
        await auditService.LogEventAsync("Locatie", "R", null, locatie?.AsResponseContract());
        return locatie?.AsResponseContract();
    }

    public async Task<LocatieResponseContract> UpdateAsync(int id, LocatieRequestContract locatie)
    {
        var locatieToUpdate = await locatieRepository.GetByIdAsync(id);
        if (locatieToUpdate == null)
        {
            throw new ArgumentException($"Locatie met id {id} bestaat niet.");
        }
        var oudeWaarde = locatieToUpdate.AsResponseContract();
        var nieuweWaarde = (await locatieRepository.UpdateAsync(locatie.AsModel(id))).AsResponseContract();
        //create audit log
        await auditService.LogEventAsync("Locatie", "U", oudeWaarde, nieuweWaarde);
        return nieuweWaarde;
    }
}
