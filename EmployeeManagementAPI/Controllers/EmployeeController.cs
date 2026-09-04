using EmployeeManagementAPI.Models;
using EmployeeManagementAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace EmployeeManagementAPI.Controllers
{
    //[Authorize]
    /// <summary>
    /// API controller that provides endpoints to manage employees.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        /// <summary>
        /// Service for employee data operations.
        /// </summary>
        private readonly IEmployeeService _employeeService;

        /// <summary>
        /// Logger for recording controller events and errors.
        /// </summary>
        private readonly ILogger<EmployeeController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmployeeController"/> class.
        /// </summary>
        /// <param name="employeeService">The employee service implementation.</param>
        /// <param name="logger">The logger instance.</param>
        public EmployeeController(IEmployeeService employeeService, ILogger<EmployeeController> logger)
        {
            _employeeService = employeeService;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves all employees.
        /// </summary>
        /// <returns>A list of employees wrapped in an <see cref="IActionResult"/>.</returns>
        [HttpGet]
        public async Task<IActionResult> GetEmployees()
        {
            try
            {
                var employees = await _employeeService.GetEmployees();

                return Ok(employees);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve employees.");
                return StatusCode(500, "An unexpected error occurred while retrieving employees.");
            }
        }

        /// <summary>
        /// Retrieves a single employee by identifier.
        /// </summary>
        /// <param name="id">The employee identifier.</param>
        /// <returns>The employee if found; otherwise NotFound.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployeeById(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid employee id.");
            }

            try
            {
                var employee = await _employeeService.GetEmployeeById(id);

                if (employee == null)
                {
                    return NotFound();
                }

                return Ok(employee);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve employee with id {Id}.", id);
                return StatusCode(500, "An unexpected error occurred while retrieving the employee.");
            }
        }

        /// <summary>
        /// Creates a new employee.
        /// </summary>
        /// <param name="employee">The employee to create.</param>
        /// <returns>The created employee or result of the create operation.</returns>
        [HttpPost]
        public async Task<IActionResult> CreateEmployee(Employee employee)
        {
            if (employee == null)
            {
                return BadRequest("Employee cannot be null.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (string.IsNullOrWhiteSpace(employee.FirstName) || string.IsNullOrWhiteSpace(employee.LastName) || string.IsNullOrWhiteSpace(employee.Email))
            {
                return BadRequest("FirstName, LastName and Email are required fields.");
            }

            try
            {
                var result = await _employeeService.CreateEmployee(employee);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create employee.");
                return StatusCode(500, "An unexpected error occurred while creating the employee.");
            }
        }

        /// <summary>
        /// Updates an existing employee.
        /// </summary>
        /// <param name="id">The identifier of the employee to update.</param>
        /// <param name="employee">The updated employee data.</param>
        /// <returns>The updated employee if successful; otherwise NotFound.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee(int id, Employee employee)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid employee id.");
            }

            if (employee == null)
            {
                return BadRequest("Employee cannot be null.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (employee.Id != 0 && employee.Id != id)
            {
                return BadRequest("Employee id in the body does not match the route id.");
            }

            if (string.IsNullOrWhiteSpace(employee.FirstName) || string.IsNullOrWhiteSpace(employee.LastName) || string.IsNullOrWhiteSpace(employee.Email))
            {
                return BadRequest("FirstName, LastName and Email are required fields.");
            }

            try
            {
                // Ensure the id is assigned to the entity
                employee.Id = id;

                var result = await _employeeService.UpdateEmployee(id, employee);

                if (result == null)
                {
                    return NotFound();
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update employee with id {Id}.", id);
                return StatusCode(500, "An unexpected error occurred while updating the employee.");
            }
        }

        /// <summary>
        /// Deletes an employee by identifier.
        /// </summary>
        /// <param name="id">The identifier of the employee to delete.</param>
        /// <returns>Ok if deleted; otherwise NotFound.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid employee id.");
            }

            try
            {
                var result = await _employeeService.DeleteEmployee(id);

                if (!result)
                {
                    return NotFound();
                }

                return Ok("Employee deleted successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete employee with id {Id}.", id);
                return StatusCode(500, "An unexpected error occurred while deleting the employee.");
            }
        }

        /// <summary>
        /// Searches for employees by first name.
        /// </summary>
        /// <param name="firstName">The first name to search for.</param>
        /// <returns>A list of employees matching the search criteria.</returns>
        [HttpGet("search")]
        public async Task<IActionResult> SearchEmployee(string firstName)
        {
            if (string.IsNullOrWhiteSpace(firstName))
            {
                return BadRequest("firstName must be provided to search.");
            }

            try
            {
                var employees = await _employeeService.SearchEmployeeAsync(firstName);

                return Ok(employees);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to search employees by first name {FirstName}.", firstName);
                return StatusCode(500, "An unexpected error occurred while searching for employees.");
            }
        }
    }
}