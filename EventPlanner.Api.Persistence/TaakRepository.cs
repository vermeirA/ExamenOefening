using System;
using EventPlanner.Api.Persistence.Models;
using Microsoft.EntityFrameworkCore;

namespace EventPlanner.Api.Persistence;

public interface ITaakRepository
{
    Task<Taak?> GetByIdAsync(int id);
    Task<List<Taak>> GetAllAsync();
    Task<Taak> AddAsync(Taak locatie);
    Task<Taak> UpdateAsync(Taak locatie);
    Task DeleteAsync(int id);
}
public class TaakRepository(EventPlannerDbContext dbContext) : ITaakRepository
{
    public async Task<Taak> AddAsync(Taak taak)
    {
        await dbContext.Taken.AddAsync(taak);
        await dbContext.SaveChangesAsync();
        await dbContext.Entry(taak).Reference(e => e.Evenement).LoadAsync();
        return taak;
    }

    public async Task DeleteAsync(int id)
    {
        var taak = await dbContext.Taken.FindAsync(id);
        if (taak != null)
        {
            dbContext.Taken.Remove(taak);
            await dbContext.SaveChangesAsync();
        }
    }

    public async Task<List<Taak>> GetAllAsync()
    {
        return await dbContext.Taken
       .Include(e => e.Evenement)
       .ToListAsync();
    }

    public async Task<Taak?> GetByIdAsync(int id)
    {
        return await dbContext.Taken
        .Include(e => e.Evenement)
        .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<Taak> UpdateAsync(Taak taak)
    {
        var existing = await dbContext.Taken
        .Include(e => e.Evenement)
        .FirstOrDefaultAsync(e => e.Id == taak.Id);

        if (existing == null)
            throw new ArgumentException($"Evenement with id {taak.Id} not found.");

        existing.Naam = taak.Naam;
        existing.EvenementId = taak.EvenementId;
        existing.Beschrijving = taak.Beschrijving;
        existing.Belangrijkheid = taak.Belangrijkheid;
        existing.Status = taak.Status;
        existing.DeadlineDateTime = taak.DeadlineDateTime;
        existing.Evenement = taak.Evenement;

        await dbContext.SaveChangesAsync();
        return existing;
    }
}
