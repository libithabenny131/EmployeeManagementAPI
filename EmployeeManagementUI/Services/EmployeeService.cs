using EmployeeManagementUI.Models;
using System.Net.Http.Json;

namespace EmployeeManagementUI.Services
{
    public class EmployeeService
    {
        private readonly HttpClient _httpClient;

        public EmployeeService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Employee>> GetEmployees()
        {
            return await _httpClient.GetFromJsonAsync<List<Employee>>
            (
                "api/employee"
            ) ?? new List<Employee>();
        }

        public async Task CreateEmployee(Employee employee)
        {
            await _httpClient.PostAsJsonAsync(
                "api/employee",
                employee);
        }

        public async Task<Employee?> GetEmployeeById(int id)
        {
            return await _httpClient.GetFromJsonAsync<Employee>
            (
                $"api/employee/{id}"
            );
        }

        public async Task UpdateEmployee(int id, Employee employee)
        {
            await _httpClient.PutAsJsonAsync(
                $"api/employee/{id}",
                employee);
        }

        public async Task DeleteEmployee(int id)
        {
            await _httpClient.DeleteAsync(
                $"api/employee/{id}"
            );
        }
    }
}