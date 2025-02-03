# Web Service Design & Implementation

Our project’s backend is implemented as a RESTful web API using ASP.NET Core and Entity Framework Core with SQLite. This design provides a robust, lightweight service that handles authentication, employee management, and worktime tracking. In this post, I will describe our web API design, provide code examples, and discuss how we use file storage for data.

## RESTful API Overview

The web API exposes endpoints for various operations, including:
- **Authentication:** An endpoint to log in and retrieve a JWT token.
- **Employee Management:** Endpoints to list, add, update, and delete employees.
- **Worktime Management:** Endpoints to add worktime entries and retrieve worktime summaries.

For instance, our **EmployeesController** is decorated with:
```csharp
[ApiController]
[Route("api/[controller]")]
public class EmployeesController : ControllerBase
{
    // CRUD operations for Employee data.
}

Below is an example of how we set up JWT authentication in our Programs.cs

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    var jwtKey = builder.Configuration["Jwt:Key"] 
        ?? throw new InvalidOperationException("JWT Key is missing in configuration.");
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };
});

API Endpoints Overview
Our API endpoints include:

/api/auth/login:
Accepts login credentials and returns a JWT token.
/api/employees:
Supports GET, POST, PUT, DELETE for employee management.
/api/worktime:
Provides endpoints to add worktime entries and retrieve worktime summaries for employees.

## File Storage and Data Persistence
We use Entity Framework Core with SQLite for data storage. Our ApplicationDbContext class is configured as follows:

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    { }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Worktime> Worktimes { get; set; }
}


The SQLite database file (app.db) is stored locally on the server. This approach allows for simple file-based storage without the complexity of a full SQL Server installation, making it ideal for small to medium projects.

In summary, our RESTful web API design follows standard conventions, making use of attribute routing, middleware for authentication and authorization, and EF Core for data persistence. The next blog post will cover the web application itself, including how our frontend interacts with this API.