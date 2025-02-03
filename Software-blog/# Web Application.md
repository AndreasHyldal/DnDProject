# Web Application

Our frontend web application is built using Blazor Server, which offers a responsive and interactive user experience. This post describes how we implemented key requirements in our web application, an overview of the pages, and how the frontend connects to our web service.

## Implementation of Key Requirements

One of the primary requirements was to allow admins to manage employees. This functionality is implemented on a dedicated page called "Employees." The page displays a grid of employee records and allows admins to add, edit, or delete employees.

Below is a sample snippet from our **Employees.razor** component:
```razor
@page "/admin/employees"
@using System.Net.Http.Json
@using FE = DndReexam.Models
@inject HttpClient Http
@attribute [Authorize(Roles = "Admin")]

<h3>Employee Management</h3>
@if (employees == null)
{
    <p><em>Loading employees...</em></p>
}
else
{
    <table class="employees-table">
        <thead>
            <tr>
                <th>First Name</th>
                <th>Last Name</th>
                <th>Email</th>
                <th>PasswordHash</th>
                <th>Role</th>
                <th>Edit</th>
            </tr>
        </thead>
        <tbody>
            @foreach (var emp in employees)
            {
                <tr>
                    <td>@emp.FirstName</td>
                    <td>@emp.LastName</td>
                    <td>@emp.Email</td>
                    <td>@emp.PasswordHash</td>
                    <td>@emp.Role</td>
                    <td>
                        <button @onclick="() => EditEmployee(emp)">Edit</button>
                    </td>
                </tr>
            }
        </tbody>
    </table>
}
<button @onclick="AddNewEmployee">Add Employee</button>
´´´

## Overview of Web Application Pages
Our web application comprises several pages:

Home Page: Provides an overview and navigation to key sections.
Employees Page: Allows admin users to manage employee records.
Worktime Summary Page: Displays charts and tables summarizing employee work hours.
Login Page: Authenticates users and retrieves JWT tokens.

## Connecting Frontend to Web Service
The frontend interacts with the backend API using an HttpClient. We encapsulate all API calls in a service class (WorktimeService) that abstracts the HTTP communication.

For example, here’s how we retrieve a list of employees:

public async Task<List<Employee>> GetAllEmployeesAsync()
{
    try
    {
        var response = await _http.GetFromJsonAsync<List<Employee>>(EmployeeApiUrl);
        return response ?? new List<Employee>();
    }
    catch (HttpRequestException ex)
    {
        Console.WriteLine($"Error fetching employees: {ex.Message}");
        return new List<Employee>();
    }
}

In our Blazor components, we inject the HttpClient and call methods from WorktimeService to fetch data from endpoints like http://localhost:5147/api/employees.

## Summary
Our frontend application meets key requirements by providing a user-friendly interface for employee and worktime management. The seamless connection to our backend API is achieved via an HttpClient, which is configured in the DI container, ensuring that data is fetched and updated reliably. In the next blog post, we will discuss user management in detail.
