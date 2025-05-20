using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace EventPlanner.Api.Persistence;

public class EventPlannerDbContext : DbContext
{
    public EventPlannerDbContext(DbContextOptions<EventPlannerDbContext> options)
        : base(options)
    {
    }
    private readonly string connectionString = "server=127.0.0.1;port=3306;database=eventplanner;user=root;password=WachtW00rd";

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        if (!options.IsConfigured)
        {
            options.UseMySql(connectionString, new MySqlServerVersion(ServerVersion.AutoDetect(connectionString)));
        }
    }
    public DbSet<Models.Evenement> Evenementen { get; set; }
    public DbSet<Models.Locatie> Locaties { get; set; }
    public DbSet<Models.Taak> Taken { get; set; }

}
