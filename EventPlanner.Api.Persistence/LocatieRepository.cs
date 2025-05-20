using System;
using EventPlanner.Api.Persistence.Models;
using Microsoft.EntityFrameworkCore;

namespace EventPlanner.Api.Persistence;

public interface ILocatieRepository
{
    Task<Locatie?> GetByIdAsync(int id);
    Task<List<Locatie>> GetAllAsync();
    Task<Locatie> AddAsync(Locatie locatie);
    Task<Locatie> UpdateAsync(Locatie locatie);
    Task DeleteAsync(int id);
}

public class LocatieRepository(EventPlannerDbContext dbContext) : ILocatieRepository
{
    public async Task<Locatie> AddAsync(Locatie locatie)
    {
        await dbContext.Locaties.AddAsync(locatie);
        await dbContext.SaveChangesAsync();
        return locatie;
    }

    public async Task DeleteAsync(int id)
    {
        var locatie = await dbContext.Locaties.FindAsync(id);
        if (locatie != null)
        {
            dbContext.Locaties.Remove(locatie);
            await dbContext.SaveChangesAsync();
        }
    }

    public async Task<List<Locatie>> GetAllAsync()
    {
        return await dbContext.Locaties.ToListAsync();
    }

    public async Task<Locatie?> GetByIdAsync(int id)
    {
        return await dbContext.Locaties.FindAsync(id);
    }

    public async Task<Locatie> UpdateAsync(Locatie locatie)
    {
        dbContext.Locaties.Update(locatie);
        await dbContext.SaveChangesAsync();
        return locatie;
    }
}
