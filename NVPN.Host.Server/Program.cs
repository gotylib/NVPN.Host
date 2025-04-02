using DAL.Context;
using Infrastructure.Interfaces;
using Infrastructure.Mapping;
using Infrastructure.SafetyService;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using NLog;
using NLog.Web;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

var logger = LogManager.Setup().LoadConfigurationFromFile("nlog.config").GetCurrentClassLogger();
try
{
    logger.Debug("Инициализация приложения");

    var builder = WebApplication.CreateBuilder(args);

    // Настройка логгирования
    builder.Logging.ClearProviders();
    builder.Logging.SetMinimumLevel(LogLevel.Trace);
    builder.Host.UseNLog();

    // Добавление сервисов в контейнер
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddAutoMapper(typeof(Mapper));
    builder.Services.AddScoped<IServersControleService, ServersControleService>();
    builder.Services.AddSingleton<Safety>();
    builder.Services.AddDbContext<VpnDbContext>(option =>
        option.UseNpgsql("Host=localhost;Port=5432;Database=vpndb;Username=postgres;Password=1234"));

    var app = builder.Build();

    app.UseDefaultFiles();
    app.UseStaticFiles();

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
}
catch (Exception exception)
{
    logger.Error(exception, "Ошибка при запуске приложения");
    throw;
}
finally
{
    LogManager.Shutdown();
}