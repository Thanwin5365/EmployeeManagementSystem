using EmployeeJoiningAPI.Models;

namespace EmployeeJoiningAPI.Services
{
    public interface IManagerAuthService
    {
        Task<Manager?> GetManagerForLoginAsync(string username);

        bool VerifyPassword(
            string storedHash,
            string password);

        Task<List<Manager>> GetAllManagersAsync();
        Task<Manager?> GetManagerByIdAsync(int managerId);

        Task<List<Employee>> GetManagerEmployeesAsync(int managerId);
    }
}