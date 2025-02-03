# User Management

User management is a crucial part of our DnDProject. In this section, we describe the types of users in our system, how we implemented the login functionality, and how resource access is controlled among different actors.

## Users in Our System

Our system primarily distinguishes between two types of users:
1. **Admin Users:**  
   Admins (e.g., Jane Doe) have full access to the system. They can manage employee data, add or remove worktime entries, and view detailed worktime reports.
2. **Regular Employees:**  
   These users can view their own worktime summaries and access limited personal data.

Jane Doe is seeded as an admin:
```csharp
new Employee
{
    Id = 2,
    FirstName = "Jane",
    LastName = "Doe",
    Email = "jane.doe@example.com",
    PasswordHash = HashPassword("Test123!"),
    Role = "Admin",
    DateOfBirth = new DateTime(1985, 10, 20),
    HireDate = new DateTime(2023, 5, 5)
}
```

This record indicates that she is an admin. However, the actual enforcement of roles is done by our authentication and authorization system.

## Implementing Login Functionality
Our login functionality is implemented in the backend using an authentication endpoint (/api/auth/login). When a user submits their credentials, the system verifies them using our EmployeeService.AuthenticateEmployeeAsync method. If the credentials are valid, the system generates a JWT token that includes the user’s role as a claim:

```csharp
var claims = new[]
{
    new Claim(JwtRegisteredClaimNames.Sub, employee.Email),
    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
    new Claim(ClaimTypes.NameIdentifier, employee.Id.ToString()),
    new Claim(ClaimTypes.Role, employee.Role ?? "User")
};
```

This token is returned to the client, which stores it and uses it for subsequent API calls.

## Access Control Between Different Actors
Resource access is managed through role-based authorization. For example:

Endpoints decorated with ```[Authorize(Roles = "Admin")]``` are accessible only to admin users.
Regular employees have restricted access, and attempting to access admin-only endpoints will result in a 401 or a custom "Access Denied" message.
Here’s an example of how we secure an API endpoint:

```
[Authorize(Roles = "Admin")]
[HttpGet("all")]
public async Task<IActionResult> GetAllEmployees()
{
    var employees = await _employeeService.GetAllEmployeesAsync();
    return Ok(employees);
}
```


This ensures that only users whose token contains the "Admin" role can access the endpoint.

## Summary
By integrating JWT-based authentication and using role-based authorization, we ensure that only authorized users can access specific resources. The login process is robust and provides the necessary role claims, while our API endpoints check these claims to enforce access control. In our next blog post, we will discuss how we manage data access using an ORM and LINQ.
