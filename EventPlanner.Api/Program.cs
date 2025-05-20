using System.Data;
using EventPlanner.Api.Persistence;
using EventPlanner.Api.Services;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddScoped<IAuditRepository, AuditRepository>();
builder.Services.AddScoped<IAuditService, AuditService>();
builder.Services.AddScoped<ILocatieRepository, LocatieRepository>();
builder.Services.AddScoped<ITaakRepository, TaakRepository>();
builder.Services.AddScoped<IEvenementRepository, EvenementRepository>();
builder.Services.AddScoped<ILocatieService, LocatieService>();
builder.Services.AddScoped<ITaakService, TaakService>();
builder.Services.AddScoped<IEvenementService, EvenementService>();
builder.Services.AddScoped<IDbConnection>(sp =>
    new MySqlConnection(builder.Configuration.GetConnectionString("MySql")));
var connectionString = builder.Configuration.GetConnectionString("MySql");

builder.Services.AddDbContext<EventPlannerDbContext>(options =>
    options.UseMySql(connectionString, new MySqlServerVersion(ServerVersion.AutoDetect(connectionString))));
builder.Services.AddControllers();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.MapControllers();

app.UseHttpsRedirection();

app.Run();
