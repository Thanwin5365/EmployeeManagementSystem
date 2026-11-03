
using EmployeeJoiningAPI.Models;

namespace EmployeeJoiningAPI.Repositories
{
    public interface IManagerRepository
    {
        Task<Manager?> GetManagerForLoginAsync(
            string username);

        Task<List<Manager>> GetAllManagersAsync();
        Task<Manager?> GetManagerByIdAsync(int managerId);

        Task<List<Employee>> GetManagerEmployeesAsync(int managerId);
    }
}
