using DAL.Context;
using DAL.Entities;
using FluentValidation;
using FluentValidation.AspNetCore;
using Infrastructure.Interfaces;
using Infrastructure.Mapping;
using Infrastructure.SafetyService;
using Infrastructure.Services;
using Infrastructure.Services.Interfaces;
using Infrastructure.Validation;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
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
    builder.Services.AddControllers()
        .AddOData(options => options
            .Select()
            .Filter()
            .OrderBy()
            .SetMaxTop(100)
            .Count()
            .Expand()
            .AddRouteComponents("odata", GetEdmModel()));
    
    builder.Services.AddFluentValidationAutoValidation();
    builder.Services.AddFluentValidationClientsideAdapters(); 
    builder.Services.AddValidatorsFromAssemblyContaining<LoginModelValidator>();
    
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddAutoMapper(typeof(Mapper));

    builder.Services.AddScoped<IServersControleService, ServersControleService>();
    builder.Services.AddScoped<IServerCommandService, ServerCommandService>();
    builder.Services.AddScoped<IUserControlService, UserControlService>();
    builder.Services.AddSingleton<SafetyService>();


    builder.Services.AddHttpClient();
    
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    
    builder.Services.AddDbContext<VpnDbContext>(options =>
        options.UseNpgsql(connectionString,
            npgsqlOptionsAction: npgsqlOptions =>
            {
                npgsqlOptions.MigrationsAssembly("DAL"); // Укажите имя сборки, содержащей миграции
            }));
    
    builder.Services.AddCors(options => 
    {
        options.AddPolicy("AllowAll", policy =>
        {
            policy.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
    });

    var app = builder.Build();

    // Применение миграций базы данных при запуске приложения
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        try
        {
            var context = services.GetRequiredService<VpnDbContext>();
            var pendingMigrations = context.Database.GetPendingMigrations().ToList();
            
            if (pendingMigrations.Any())
            {
                logger.Info($"Найдено непримененных миграций: {pendingMigrations.Count}");
                foreach (var migration in pendingMigrations)
                {
                    logger.Info($"  - {migration}");
                }
                logger.Info("Применение миграций базы данных...");
                context.Database.Migrate();
                logger.Info("Миграции успешно применены.");
            }
            else
            {
                logger.Info("Все миграции уже применены. Пропускаем применение миграций.");
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex, "Ошибка при применении миграций базы данных");
            throw;
        }
    }

    app.UseDefaultFiles();
    app.UseStaticFiles();

    if (app.Environment.IsDevelopment()|| app.Environment.IsProduction())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.UseRouting();
    app.UseCors("AllowAll");
    app.UseAuthorization();
    app.UseMiddleware<CustomExceptionHandlingMiddleware>();
    
    app.MapControllers();
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

static IEdmModel GetEdmModel()
{
    var builder = new ODataConventionModelBuilder();
    builder.EntitySet<User>("Users");
    return builder.GetEdmModel();
}