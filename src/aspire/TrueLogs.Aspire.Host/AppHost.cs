
var builder = DistributedApplication.CreateBuilder(args);

var roviderHost = builder
    .AddProject<Projects.TrueLogs_Api_Provider_Host>("truelogs-api-provider-host");

var emitterHost = builder
    .AddProject<Projects.TrueLogs_Api_Emitter_Host>("truelogs-api-emitter-host");

var webHost = builder
    .AddProject<Projects.TrueLogs_Web_Host>("truelogs-web-host");

builder
    .Build()
    .Run();
