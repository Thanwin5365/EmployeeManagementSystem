using EmployeeJoiningAPI.Models;
using EmployeeJoiningAPI.Repositories;

namespace EmployeeJoiningAPI.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IPasswordService _passwordService;

        public EmployeeService(
            IEmployeeRepository employeeRepository,
            IPasswordService passwordService)
        {
            _employeeRepository = employeeRepository;
            _passwordService = passwordService;
        }

        public async Task<Employee?> GetEmployeeForLoginAsync(
            string username)
        {
            return await _employeeRepository
                .GetEmployeeForLoginAsync(username);
        }

        public async Task<List<Employee>> GetAllEmployeesAsync()
        {
            return await _employeeRepository
                .GetAllEmployeesAsync();
        }

        public async Task<Employee?> GetEmployeeByIdAsync(
            int employeeId)
        {
            return await _employeeRepository
                .GetEmployeeByIdAsync(employeeId);
        }

        public async Task<bool> ValidateUser(
    string username,
    string password)
        {
            var storedHash =
                await _employeeRepository.GetPasswordHashAsync(username);

            if (string.IsNullOrEmpty(storedHash))
            {
                return false;
            }

            return _passwordService.VerifyPassword(
                storedHash,
                password);
        }

        public string HashPassword(string password)
        {
            return _passwordService.HashPassword(password);
        }
    }
}