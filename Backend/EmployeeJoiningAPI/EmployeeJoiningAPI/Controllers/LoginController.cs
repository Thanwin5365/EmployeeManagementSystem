using EmployeeJoiningAPI.Models;
using EmployeeJoiningAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeJoiningAPI.Controllers
{
    [ApiController]
    [Route("login")]
    public class LoginController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly IPasswordService _passwordService;

        public LoginController(
            IEmployeeService employeeService,
            IPasswordService passwordService)
        {
            _employeeService = employeeService;
            _passwordService = passwordService;
        }


        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var empValid = await _employeeService.ValidateUser(
    request.Username,
    request.Password);

            if (empValid != true)
            {
                return Unauthorized(
                    "Invalid Username or Password.");
            }

            var employee =
                await _employeeService.GetEmployeeForLoginAsync(
                    request.Username);

            if (employee == null)
            {
                return Unauthorized(
                    "Invalid Username or Password.");
            }

            return Ok(new
            {
                message = "Login successful",
                employeeId = employee.EmployeeId,
                username = employee.Username,
                name = employee.Name,
                department = employee.Department
            });
        }
    }
}