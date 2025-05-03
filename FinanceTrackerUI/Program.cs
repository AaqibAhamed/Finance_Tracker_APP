using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using FinanceTrackerUI;
using FinanceTrackerUI.Services;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5243/") }); // Use your API base URL

builder.Services.AddMudServices(); // Register MudBlazor services

// Register our services
builder.Services.AddScoped<IncomeService>();
builder.Services.AddScoped<ExpenseService>();

await builder.Build().RunAsync();