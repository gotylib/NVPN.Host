using DAL.Context;
using Infrastructure.Interfaces;
using Infrastructure.Mapping;
using Infrastructure.SafetyService;
using Infrastructure.Services;
using Infrastructure.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using NLog;
using NLog.Web;
using NVPN.Host.Server.CastomMiddleware;
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
    builder.Services.AddScoped<IServerCommandService, ServerCommandService>();
    builder.Services.AddScoped<IUserControleService, UserControleService>();
    builder.Services.AddScoped<IUserControleService, UserControleService>();
    builder.Services.AddSingleton<SafetyService>();


    builder.Services.AddHttpClient();
    
    // builder.Configuration
    //     .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    //     .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true);
    builder.Configuration.AddEnvironmentVariables();
    var connectionString =  "Host=localhost;Port=5432;Database=vpndb;Username=postgres;Password=1234";

    builder.Services.AddCors(options => 
    {
        options.AddPolicy("AllowAll", policy =>
        {
            policy.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
    });
    
    builder.Services.AddDbContext<VpnDbContext>(options =>
        options.UseNpgsql(connectionString,
            npgsqlOptionsAction: npgsqlOptions =>
            {
                npgsqlOptions.MigrationsAssembly("DAL"); // Укажите имя сборки, содержащей миграции
            }));

    var app = builder.Build();

    app.UseDefaultFiles();
    app.UseStaticFiles();

    if (app.Environment.IsDevelopment()|| app.Environment.IsProduction())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers();
    app.UseRouting();
    app.UseMiddleware<CustomExceptionHandlingMiddleware>();
    app.MapFallbackToFile("/index.html");
    app.UseCors("AllowAll");
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