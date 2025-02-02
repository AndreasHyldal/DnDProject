using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Backend.Data;
using Backend.Services;

var builder = WebApplication.CreateBuilder(args);

// ✅ Register Services
builder.Services.AddScoped<WorktimeService>(); 

// ✅ Register SQLite Database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite("Data Source=app.db"));

// ✅ Enable CORS for Blazor frontend to access API
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy => policy.WithOrigins("http://localhost:5075") // Ensure this matches Blazor frontend
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});

// ✅ Register Controllers & API Endpoints
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "DnDProject API",
        Version = "v1",
        Description = "API for managing timesheet of workers.",
    });

    // ✅ Add JWT Authentication to Swagger
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer {your JWT token}'",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            new string[] {}
        }
    });
});

var app = builder.Build();

// ✅ Configure Middleware
app.UseRouting(); // 🔹 Enable Routing
app.UseCors("AllowFrontend"); // 🔹 Apply CORS
app.UseHttpsRedirection(); // 🔹 Enforce HTTPS (Optional)
app.UseAuthentication(); // 🔹 Enable Authentication (If using JWT)
app.UseAuthorization(); // 🔹 Enable Authorization for secure endpoints
app.UseSwagger();
app.UseSwaggerUI();

// ✅ Map API Controllers
app.MapControllers();

// ✅ Enable OpenAPI Docs in Development
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// ✅ Start the App
app.Run();
