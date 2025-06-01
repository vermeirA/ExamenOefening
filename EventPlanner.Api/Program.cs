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

var connectionStringSql = builder.Configuration.GetConnectionString("MySql");
var connectionStringMongo = builder.Configuration.GetConnectionString("MongoDb");
builder.Services.AddDbContext<EventPlannerDbContext>(options =>
    options.UseMySql(connectionStringSql, new MySqlServerVersion(ServerVersion.AutoDetect(connectionStringSql))));
builder.Services.AddDbContext<AuditDbContext>(options =>
    options.UseMongoDB(connectionStringMongo, "eventplanner_audit"));

builder.Services.AddControllers().AddMvcOptions(options =>
{
    options.SuppressAsyncSuffixInActionNames = false;
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.MapControllers();

app.UseHttpsRedirection();

app.Run();
