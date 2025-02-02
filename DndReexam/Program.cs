using DndReexam.Components;
using Havit.Blazor.Components.Web;
using MudBlazor.Services;
using DndReexam.Services; // Import your AuthService namespace
using Blazored.LocalStorage; // Import Blazored.LocalStorage for LocalStorageService

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddScoped<WorktimeService>();

builder.Services.AddHxServices();
builder.Services.AddMudServices();
builder.Services.AddBlazoredLocalStorage(); // Register Blazored.LocalStorage
builder.Services.AddScoped<IAuthService, AuthService>(); // Register AuthService
builder.Services.AddHttpClient(); // Register HttpClient

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
