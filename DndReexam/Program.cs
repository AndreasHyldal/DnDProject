using DndReexam.Components;
using Havit.Blazor.Components.Web;
using MudBlazor.Services;
using DndReexam.Services; // Import your AuthService namespace
using Blazored.LocalStorage; // Import Blazored.LocalStorage for LocalStorageService

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Register WorktimeService
builder.Services.AddScoped<WorktimeService>();

// Add UI frameworks and local storage service
builder.Services.AddHxServices();
builder.Services.AddMudServices();
builder.Services.AddBlazoredLocalStorage();

// Register AuthService and HttpClient
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5147/") });

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts(); // The default HSTS value is 30 days. You may want to change this for production scenarios.
}

app.UseHttpsRedirection();
app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
