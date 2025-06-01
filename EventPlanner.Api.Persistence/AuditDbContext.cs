using System;
using EventPlanner.Shared;
using Microsoft.EntityFrameworkCore;
using MongoDB.EntityFrameworkCore.Extensions;

namespace EventPlanner.Api.Persistence;

public class AuditDbContext : DbContext
{
    public DbSet<AuditEntry> AuditLogs { get; set; }

    public AuditDbContext(DbContextOptions<AuditDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AuditEntry>().ToCollection("auditlog");
    }
}
