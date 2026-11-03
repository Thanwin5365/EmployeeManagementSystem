
using EmployeeJoiningAPI.Models;
using EmployeeJoiningAPI.Repositories;

namespace EmployeeJoiningAPI.Services
{
    public class ManagerAuthService : IManagerAuthService
    {
        private readonly IManagerRepository _managerRepository;
        private readonly IPasswordService _passwordService;

        public ManagerAuthService(
            IManagerRepository managerRepository,
            IPasswordService passwordService)
        {
            _managerRepository = managerRepository;
            _passwordService = passwordService;
        }

        public async Task<Manager?> GetManagerForLoginAsync(
            string username)
        {
            return await _managerRepository
                .GetManagerForLoginAsync(username);
        }

        public async Task<List<Manager>> GetAllManagersAsync()
        {
            return await _managerRepository
                .GetAllManagersAsync();
        }

        public async Task<Manager?> GetManagerByIdAsync(
            int managerId)
        {
            return await _managerRepository
                .GetManagerByIdAsync(managerId);
        }

        public async Task<List<Employee>> GetManagerEmployeesAsync(
            int managerId)
        {
            return await _managerRepository
                .GetManagerEmployeesAsync(managerId);
        }

        public bool VerifyPassword(
            string storedHash,
            string password)
        {
            return _passwordService.VerifyPassword(
                storedHash,
                password);
        }
    }
}
