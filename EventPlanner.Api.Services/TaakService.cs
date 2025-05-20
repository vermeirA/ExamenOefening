using System;
using EventPlanner.Api.Contracts.Taak;
using EventPlanner.Api.Persistence;
using EventPlanner.Api.Services.Mapping;


namespace EventPlanner.Api.Services;

public interface ITaakService
{
    Task<TaakResponseContract?> GetByIdAsync(int id);
    Task<List<TaakResponseContract>> GetAllAsync();
    Task<TaakResponseContract> AddAsync(TaakRequestContract taak);
    Task<TaakResponseContract> UpdateAsync(int id, TaakRequestContract taak);
    Task DeleteAsync(int id);
    Task PatchStatusAsync(int id, TaakPatchStatusRequestContract request);
}

public class TaakService(ITaakRepository taakRepository, IEvenementRepository evenementRepository, IAuditService auditService) : ITaakService
{
    public async Task<TaakResponseContract> AddAsync(TaakRequestContract taak)
    {
        var foundEvent = await evenementRepository.GetByIdAsync(taak.EvenementId);
        if (foundEvent == null)
        {
            throw new ArgumentException($"Evenement met id {taak.EvenementId} bestaat niet.");
        }
        var addedTaak = (await taakRepository.AddAsync(taak.AsModel(foundEvent))).AsResponseContract();
        await auditService.LogEventAsync("Taak", "C", null, addedTaak);
        return addedTaak;
    }

    public async Task DeleteAsync(int id)
    {
        var taakToDelete = await taakRepository.GetByIdAsync(id);
        if (taakToDelete == null)
        {
            throw new ArgumentException($"Taak met id {id} bestaat niet.");
        }
        await auditService.LogEventAsync("Taak", "D", taakToDelete.AsResponseContract(), null);
        await taakRepository.DeleteAsync(id);
    }

    public async Task<List<TaakResponseContract>> GetAllAsync()
    {
        var taak = await taakRepository.GetAllAsync();
        var taakList = taak.Select(l => l.AsResponseContract()).ToList();
        await auditService.LogEventAsync("Taak", "R", null, taakList);
        return taakList;
    }

    public async Task<TaakResponseContract?> GetByIdAsync(int id)
    {
        var taak = await taakRepository.GetByIdAsync(id);
        var response = taak?.AsResponseContract();
        await auditService.LogEventAsync("Taak", "R", null, response);
        return response;
    }

    public async Task<TaakResponseContract> UpdateAsync(int id, TaakRequestContract taak)
    {
        var taakToUpdate = await taakRepository.GetByIdAsync(id);
        if (taakToUpdate == null) throw new ArgumentException($"Taak met id {id} bestaat niet.");

        var foundEvent = await evenementRepository.GetByIdAsync(taak.EvenementId);
        if (foundEvent == null) throw new ArgumentException($"Evenement met id {taak.EvenementId} bestaat niet.");

        var oudeWaarde = taakToUpdate.AsResponseContract();
        var nieuweWaarde = (await taakRepository.UpdateAsync(taak.AsModel(foundEvent, id))).AsResponseContract();
        await auditService.LogEventAsync("Taak", "U", oudeWaarde, nieuweWaarde);
        return nieuweWaarde;
    }

    public async Task PatchStatusAsync(int id, TaakPatchStatusRequestContract request)
    {
        var taak = await taakRepository.GetByIdAsync(id);
        if (taak == null || request == null)
        {
            throw new Exception($"Taak with id {id} not found or request is null.");
        }
        var oudeWaarde = taak.Status;
        taak.Status = request.Status;
        await auditService.LogEventAsync("Taak", "U", oudeWaarde, request.Status);
        await taakRepository.UpdateAsync(taak);
    }
}
