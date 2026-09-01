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
                "https://localhost:7040/api/employee"
            ) ?? new List<Employee>();
        }

        public async Task CreateEmployee(Employee employee)
        {
            await _httpClient.PostAsJsonAsync(
                "https://localhost:7040/api/employee",
                employee);
        }

        public async Task<Employee?> GetEmployeeById(int id)
        {
            return await _httpClient.GetFromJsonAsync<Employee>(
                $"https://localhost:7040/api/employee/{id}");
        }

        public async Task UpdateEmployee(int id, Employee employee)
        {
            await _httpClient.PutAsJsonAsync(
                $"https://localhost:7040/api/employee/{id}",
                employee);
        }

        public async Task DeleteEmployee(int id)
        {
            await _httpClient.DeleteAsync(
                $"https://localhost:7040/api/employee/{id}");
        }
    }
}