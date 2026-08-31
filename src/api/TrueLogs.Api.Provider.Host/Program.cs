using TrueLogs.Storage.LiteDB.Logs.Managers;
using TrueLogs.Storage.LiteDB.Logs.Repositories;
using TrueLogs.Storage.Logs.Managers;
using TrueLogs.Api.Provider.Host.Configurations;
using TrueLogs.Api.Core;

var builder = WebApplication
    .CreateBuilder(args)
    .AddAspire()
    .AddLogging()
    .AddDevelopmentPolicyCors()
    .AddEmitterAdapterFile();

builder.Services.AddScoped<LiteDbLogRepository, LiteDbLogRepository>();
builder.Services.AddLogProvider();
builder.Services.AddScoped<ILogManager, LiteDBLogManager>();
//builder.Services.AddHostedService<FileWatcherBackgroundService>();

builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddControllers();

var application = builder
    .Build()
    .AddAspire();

if (application.Environment.IsDevelopment())
{
    application.UseSwagger();
    application.UseSwaggerUI();

    application.UseCors(CorsExtentions.DevelopmentPolicyCorsName);
}

application.UseHttpsRedirection();

// TODO временно убираем
application.UseAuthorization();

application.MapControllers();

application.Run();
