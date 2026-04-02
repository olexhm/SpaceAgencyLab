using DevOpsSpaceAgency.Web;
using DevOpsSpaceAgency.Web.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddMudServices();
builder.Services.AddScoped(_ => new HttpClient { BaseAddress = new Uri("https://localhost:7193/") });
builder.Services.AddScoped<SpaceApiClient>();

await builder.Build().RunAsync();
