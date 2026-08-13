using EmployeeManagementAPI.Data;
using EmployeeManagementAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementAPI.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly EmployeeDbContext _context;
        private readonly ILogger<EmployeeRepository> _logger;

        public EmployeeRepository(EmployeeDbContext context, ILogger<EmployeeRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<Employee>> GetAllAsync()
        {
            _logger.LogInformation("Fetching employees from database");
            return await _context.Employees.ToListAsync();
        }

        public async Task<Employee?> GetByIdAsync(int id)
        {
            _logger.LogInformation("GetEmployee from database for Id {Id}\", id");
            return await _context.Employees
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<Employee> CreateAsync(Employee employee)
        {
            _logger.LogInformation("CreateEmployee on database");
            _context.Employees.Add(employee);

            await _context.SaveChangesAsync();

            return employee;
        }

        public async Task<Employee?> UpdateAsync(int id, Employee employee)
        {
            _logger.LogInformation("UpdateEmployee on database for Id {Id}", id);
            var existingEmployee = await _context.Employees
                .FirstOrDefaultAsync(e => e.Id == id);

            if (existingEmployee == null)
            {
                return null;
            }

            existingEmployee.FirstName = employee.FirstName;
            existingEmployee.LastName = employee.LastName;
            existingEmployee.Email = employee.Email;
            existingEmployee.Salary = employee.Salary;

            await _context.SaveChangesAsync();

            return existingEmployee;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            _logger.LogInformation("DeleteEmployee ondatabase for Id {Id}", id);
            var employee = await _context.Employees
                .FirstOrDefaultAsync(e => e.Id == id);

            if (employee == null)
            {
                return false;
            }

            _context.Employees.Remove(employee);

            await _context.SaveChangesAsync();

            return true;
        }
    }
}