using EmployeeJoiningAPI.Models;

namespace EmployeeJoiningAPI.Services
{
    public interface IEmployeeService
    {
        // Login
        Task<Employee?> GetEmployeeForLoginAsync(string username);

        // Get all employees
        Task<List<Employee>> GetAllEmployeesAsync();

        // Get employee by ID
        Task<Employee?> GetEmployeeByIdAsync(int employeeId);

        Task<bool> ValidateUser(string username, string password);

        string HashPassword(string password);
        
    }
}