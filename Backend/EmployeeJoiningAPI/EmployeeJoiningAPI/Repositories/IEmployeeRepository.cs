using EmployeeJoiningAPI.Models;

namespace EmployeeJoiningAPI.Repositories
{
    public interface IEmployeeRepository
    {
        Task<bool> ValidateUser(
            string username,
            string hashedPassword);

        Task<Employee?> GetEmployeeForLoginAsync(
            string username);

        Task<List<Employee>> GetAllEmployeesAsync();

        Task<Employee?> GetEmployeeByIdAsync(
           int employeeId);
        Task<string?> GetPasswordHashAsync(string username);
    }
}
