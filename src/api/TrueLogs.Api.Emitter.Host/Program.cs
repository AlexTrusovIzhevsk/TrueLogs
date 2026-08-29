using TrueLogs.Storage.LiteDB;
using TrueLogs.Storage.LiteDB.Logs.Managers;
using TrueLogs.Storage.LiteDB.Logs.Repositories;
using TrueLogs.Storage.Logs.Managers;
using TrueLogs.Storage.Logs.Providers;
using TrueLogs.Api.Emitter.Host.Configurations;
using TrueLogs.Api.Host.Configurations;

var builder = WebApplication
    .CreateBuilder(args)
    .AddAspire()
    .AddLogging()
    .AddDevelopmentPolicyCors();

builder.Services.AddScoped<LiteDbLogRepository, LiteDbLogRepository>();
builder.Services.AddScoped<LogProvider, LiteDBLogProvider>();
builder.Services.AddScoped<ILogManager, LiteDBLogManager>();

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
