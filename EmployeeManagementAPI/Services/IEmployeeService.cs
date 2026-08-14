using EmployeeManagementAPI.Models;

namespace EmployeeManagementAPI.Services
{
    public interface IEmployeeService
    {
        Task<List<Employee>> GetEmployees();

        Task<Employee?> GetEmployeeById(int id);

        Task<Employee> CreateEmployee(Employee employee);

        Task<Employee?> UpdateEmployee(int id, Employee employee);

        Task<bool> DeleteEmployee(int id);

        Task<List<Employee>> SearchEmployeeAsync(string firstName);
    }
}