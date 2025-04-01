using DAL.Context;
using Infrastructure.Interfaces;
using Infrastructure.Mapping;
using Infrastructure.SafetyService;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(Mapper));
builder.Services.AddScoped<IServersControleService, ServersControleService>();
builder.Services.AddSingleton<Safety>();

builder.Services.AddDbContext<VpnDbContext>(option => option.UseNpgsql("Host=localhost;Port=5432;Database=vpndb;Username=postgres;Password=1234"));

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseRouting();

app.MapFallbackToFile("/index.html");

app.Run();