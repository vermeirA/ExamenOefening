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
    
    public DbSet<Models.Evenement> Evenementen { get; set; }
    public DbSet<Models.Locatie> Locaties { get; set; }
    public DbSet<Models.Taak> Taken { get; set; }

}
