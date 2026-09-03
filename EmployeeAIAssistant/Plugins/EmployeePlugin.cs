using EmployeeAIAssistant.Models;
using Microsoft.SemanticKernel;
using System.Net.Http.Json;
using System.ComponentModel;

namespace EmployeeAIAssistant.Plugins;

public class EmployeePlugin
{
    private readonly HttpClient _httpClient;

    public EmployeePlugin()
    {
        _httpClient = new HttpClient();
    }

    [KernelFunction]
    public async Task<string> GetEmployeeCount()
    {
        var employees =
            await _httpClient.GetFromJsonAsync<List<Employee>>
            (
                "https://localhost:7040/api/employee"
            );

        return $"Total Employees: {employees?.Count}";
    }

    [KernelFunction]
    public async Task<string> GetHighestSalaryEmployee()
    {
        var employees =
            await _httpClient.GetFromJsonAsync<List<Employee>>
            (
                "https://localhost:7040/api/employee"
            );

        var employee = employees?
            .OrderByDescending(e => e.Salary)
            .FirstOrDefault();

        if (employee == null)
        {
            return "No employees found.";
        }

        return $"{employee.FirstName} earns the highest salary of {employee.Salary}";
    }

    [KernelFunction]

    [Description("Get employee details by employee id")]
    public async Task<string> GetEmployeeById(int id)
    {
        var response =
            await _httpClient.GetAsync(
                $"https://localhost:7040/api/employee/{id}");

        if (!response.IsSuccessStatusCode)
        {
            return $"Employee with Id {id} was not found.";
        }

        var employee =
            await response.Content
                .ReadFromJsonAsync<Employee>();

        return $"""
            Employee Details:

            Id: {employee!.Id}
            Name: {employee.FirstName} {employee.LastName}
            Email: {employee.Email}
            Salary: {employee.Salary}
            """;
    }
    [KernelFunction]
    [Description("Get employees whose salary is greater than the specified amount")]
    public async Task<string> GetEmployeesAboveSalary(decimal salary)
    {
        var employees =
            await _httpClient.GetFromJsonAsync<List<Employee>>
            (
                "https://localhost:7040/api/employee"
            );

        var filteredEmployees =
            employees?
            .Where(e => e.Salary > salary)
            .ToList();

        if (filteredEmployees == null ||
            !filteredEmployees.Any())
        {
            return "No employees found.";
        }

        return string.Join(
            Environment.NewLine,
            filteredEmployees.Select(
                e => $"{e.FirstName} - {e.Salary}"));
    }
}