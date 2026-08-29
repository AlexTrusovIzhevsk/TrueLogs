using Fluxor;
using Fluxor.Blazor.Web.ReduxDevTools;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using TrueLogs.Contract.Clients;
using TrueLogs.Web.Client;
using TrueLogs.Web.Host;
using TrueLogs.Web.Store;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddFluxor(options =>
{
    var assembly = typeof(StoreAnchor).Assembly;
    options.ScanAssemblies(assembly);
    options.UseReduxDevTools();
});

builder.Services.AddMudServices();
builder.Services.AddScoped<ILogProviderClient, LogProviderClient>();


builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5195/") });

await builder
    .Build()
    .RunAsync();