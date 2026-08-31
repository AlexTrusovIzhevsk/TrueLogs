using TrueLogs.Storage.LiteDB.Logs.Managers;
using TrueLogs.Storage.LiteDB.Logs.Repositories;
using TrueLogs.Storage.Logs.Managers;
using TrueLogs.Api.Emitter.Host.Configurations;
using TrueLogs.Api.Host.Configurations;
using TrueLogs.Api.Core;

var builder = WebApplication
    .CreateBuilder(args)
    .AddAspire()
    .AddLogging()
    .AddDevelopmentPolicyCors();

// TODO отказатся от абстракции реопзитория
builder.Services.AddScoped<LiteDbLogRepository, LiteDbLogRepository>();
builder.Services.AddLogProvider();
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
