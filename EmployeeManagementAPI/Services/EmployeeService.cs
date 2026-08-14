using EmployeeManagementAPI.Models;
using EmployeeManagementAPI.Repositories;

namespace EmployeeManagementAPI.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILogger<EmployeeService> _logger;

        public EmployeeService(IEmployeeRepository employeeRepository, ILogger<EmployeeService> logger)
        {
            _employeeRepository = employeeRepository;
            _logger = logger;
        }

        public async Task<List<Employee>> GetEmployees()
        {
            _logger.LogInformation("GetEmployees called");
            return await _employeeRepository.GetAllAsync();
        }

        public async Task<Employee?> GetEmployeeById(int id)
        {
            _logger.LogInformation("GetEmployeeById called for Id {Id}", id);
            return await _employeeRepository.GetByIdAsync(id);
        }

        public async Task<Employee> CreateEmployee(Employee employee)
        {
            _logger.LogInformation("CreateEmployee called");
            return await _employeeRepository.CreateAsync(employee);
        }

        public async Task<Employee?> UpdateEmployee(int id, Employee employee)
        {
            _logger.LogInformation("UpdateEmployee called for Id {Id}", id);
            return await _employeeRepository.UpdateAsync(id, employee);
        }

        public async Task<bool> DeleteEmployee(int id)
        {
            _logger.LogInformation("DeleteEmployee called for Id {Id}", id);
            return await _employeeRepository.DeleteAsync(id);
        }

        public async Task<List<Employee>> SearchEmployeeAsync(string firstName)
        {
            _logger.LogInformation("Search Employee called using {firstName}", firstName);
            return await _employeeRepository.SearchEmployeeAsync(firstName);
        }
    }
}