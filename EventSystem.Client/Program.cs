using Blazored.SessionStorage;
using EventSystem.Client;
using EventSystem.Client.Helpers;
using EventSystem.Client.Store.Event;
using EventSystem.Services;
using Fluxor;
using Fluxor.Blazor.Web.ReduxDevTools;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// get api base address from config
var configuration = builder.Configuration;
var apiBaseAddress = configuration["ApiBaseAddress"];

// builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(apiBaseAddress) });

// add Services to DI
builder.Services.AddScoped<IEventService>(sp => new EventService(sp.GetRequiredService<HttpClient>()));
builder.Services.AddScoped<IAccountService>(sp => new AccountService(sp.GetRequiredService<HttpClient>()));
builder.Services.AddScoped<IEventRegistrationService>(sp => new EventRegistrationService(sp.GetRequiredService<HttpClient>()));
builder.Services.AddScoped<SessionHelper>();
builder.Services.AddBlazoredSessionStorage();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
builder.Services.AddAuthorizationCore();


// add fluxor service
builder.Services.AddFluxor(o =>
{
    o.ScanAssemblies(typeof(Program).Assembly);
    o.UseReduxDevTools(rdt =>
    {
        rdt.Name = "EventSystem.Client";
    });
});

// add effcets
builder.Services.AddScoped<EventEffects>();

await builder.Build().RunAsync();
