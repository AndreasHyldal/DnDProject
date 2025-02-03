# Data Access

Data access is a critical part of our DnDProject. In this section, we explain how the introduction of an ORM (Entity Framework Core) has changed our approach to data management, as well as how using LINQ compares to the traditional SQL approach.

## ORM and Data Access

Before using an ORM, data access involved writing raw SQL queries, managing connections, and handling database objects manually. This approach can be error-prone and difficult to maintain. With the introduction of Entity Framework Core, we work with strongly-typed classes and LINQ queries, which provide a more intuitive and robust approach to data access.

Our **ApplicationDbContext** class is defined as:
```csharp
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Worktime> Worktimes { get; set; }
}
```

This class maps our C# models (e.g., Employee, Worktime) to database tables automatically, eliminating the need for manual SQL table creation.

## Using LINQ vs. SQL
LINQ (Language Integrated Query) allows us to write queries in C# that are translated into SQL by EF Core. For example, to retrieve all employees, instead of writing raw SQL like:
```SELECT * FROM Employees```

We write:
```var employees = await _context.Employees.ToListAsync();```

LINQ queries are strongly-typed, compile-time checked, and easier to maintain. They also allow us to compose queries dynamically, such as filtering and ordering results:
```var activeEmployees = await _context.Employees
    .Where(e => e.IsActive)
    .OrderBy(e => e.LastName)
    .ToListAsync();
```

## Benefits of ORM and LINQ
Productivity:
Developers can work in C# without switching contexts to SQL.
Safety:
Compile-time checking reduces runtime errors.
Maintainability:
Changes to the data model are automatically reflected in queries.
Abstraction:
The ORM abstracts away database-specific details, making it easier to switch databases if needed.

## Code Example: Updating an Employee Record
Below is an example of updating an employee record using EF Core:
public async Task<bool> UpdateEmployeeAsync(Employee employee)
```{
    _context.Entry(employee).State = EntityState.Modified;
    try
    {
        await _context.SaveChangesAsync();
        return true;
    }
    catch (DbUpdateConcurrencyException)
    {
        if (!await _context.Employees.AnyAsync(e => e.Id == employee.Id))
        {
            return false;
        }
        else
        {
            throw;
        }
    }
}
```

This code handles data updates without writing any SQL, and the ORM manages concurrency and state tracking for us.

## Conclusion
Using EF Core and LINQ has streamlined our data access layer. The ORM provides a more natural way to work with data in C#, reduces boilerplate code, and improves maintainability compared to traditional SQL-based approaches. In our final blog post, we will summarize the project outcome and provide a demonstration of the application in use.
