using System;
using EventPlanner.Api.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using EventPlanner.Shared;
using System.Data;
using Dapper;

namespace EventPlanner.Api.Persistence;

public interface IEvenementRepository
{
    Task<Evenement?> GetByIdAsync(int id);
    Task<List<Evenement>> GetAllAsync();
    Task<Evenement> AddAsync(Evenement locatie);
    Task<Evenement> UpdateAsync(Evenement locatie);
    Task DeleteAsync(int id);
    Task<OverzichtRapportDto> GetOverzichtRapportAsync(int id);
}

public class EvenementRepository(EventPlannerDbContext dbContext, IDbConnection dbConnection) : IEvenementRepository
{
    public async Task<Evenement> AddAsync(Evenement evenement)
    {
        await dbContext.Evenementen.AddAsync(evenement);
        await dbContext.SaveChangesAsync();
        await dbContext.Entry(evenement).Reference(e => e.Locatie).LoadAsync();
        return evenement;
    }

    public async Task DeleteAsync(int id)
    {
        var evenement = await dbContext.Evenementen.FindAsync(id);
        if (evenement != null)
        {
            dbContext.Evenementen.Remove(evenement);
            await dbContext.SaveChangesAsync();
        }
    }

    public async Task<List<Evenement>> GetAllAsync()
    {
        return await dbContext.Evenementen
       .Include(e => e.Locatie)
       .ToListAsync();
    }

    public async Task<Evenement?> GetByIdAsync(int id)
    {
        return await dbContext.Evenementen
        .Include(e => e.Locatie)
        .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<Evenement> UpdateAsync(Evenement evenement)
    {
        var existing = await dbContext.Evenementen
        .Include(e => e.Locatie)
        .FirstOrDefaultAsync(e => e.Id == evenement.Id);

        if (existing == null)
            throw new ArgumentException($"Evenement with id {evenement.Id} not found.");

        existing.Naam = evenement.Naam;
        existing.StartDateTime = evenement.StartDateTime;
        existing.EindDateTime = evenement.EindDateTime;
        existing.Locatie = evenement.Locatie;

        await dbContext.SaveChangesAsync();
        return existing;
    }

    //dapper method for event report, obtainable by event id
    public async Task<OverzichtRapportDto> GetOverzichtRapportAsync(int id)
    {
        var sql = @"
            SELECT 
    e.Id AS EvenementId,
    e.Naam AS EvenementNaam,
    l.Id AS LocatieId,
    l.Naam AS LocatieNaam,
    ROUND(COALESCE(CAST(SUM(CASE WHEN t.Status = @DoneStatus THEN 1 ELSE 0 END) AS FLOAT) / NULLIF(COUNT(t.Id), 0) * 100, 0), 2) AS PercentageTakenVoltooid,
    SUM(CASE WHEN COALESCE(t.Belangrijkheid, -1) = @MustBelangrijkheid AND COALESCE(t.Status, -1) = @TodoStatus THEN 1 ELSE 0 END) AS AantalMustTakenInTodo,
    COALESCE((
        SELECT MAX(LastUpdated) 
        FROM (
            SELECT LastUpdated FROM Evenementen WHERE Id = e.Id
            UNION ALL
            SELECT LastUpdated FROM Taken WHERE EvenementId = e.Id
        ) updates
    ), '1900-01-01') AS LaatsteUpdate
FROM Evenementen e
INNER JOIN Locaties l ON e.LocatieId = l.Id
LEFT JOIN Taken t ON t.EvenementId = e.Id
WHERE e.Id = @Id
GROUP BY e.Id, e.Naam, l.Id, l.Naam;
        ";

        return await dbConnection.QuerySingleAsync<OverzichtRapportDto>(sql, new
        {
            Id = id,
            DoneStatus = (int)StatusEnum.Done,
            TodoStatus = (int)StatusEnum.Todo,
            MustBelangrijkheid = (int)BelangrijkheidEnum.Must
        });
    }
}

