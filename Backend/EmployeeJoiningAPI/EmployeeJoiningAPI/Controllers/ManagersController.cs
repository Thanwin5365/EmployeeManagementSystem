
using EmployeeJoiningAPI.Services;
using Microsoft.AspNetCore.Mvc;


namespace EmployeeJoiningAPI.Controllers
{
    [ApiController]
    [Route("managers")]
    public class ManagersController : ControllerBase
    {
        private readonly IManagerAuthService _managerAuthService;



        public ManagersController(
    IManagerAuthService managerAuthService)
        {
            _managerAuthService = managerAuthService;
        }


        // GET: /managers
        [HttpGet]
       
        public async Task<IActionResult> GetManagers()
        {
            var managers = await _managerAuthService.GetAllManagersAsync();

            return Ok(managers);
        }

        // GET: /managers/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetManager(int id)
        {
            var manager = await _managerAuthService.GetManagerByIdAsync(id);

            if (manager == null)
            {
                return NotFound(new
                {
                    message = "Manager not found."
                });
            }

            return Ok(manager);
        }

        // POST: /managers/login

        [HttpPost("login")]
        public async Task<IActionResult> Login(
            [FromBody] ManagerLoginRequest request)
        {
            var manager = await _managerAuthService
                .GetManagerForLoginAsync(request.Username);

            if (manager == null)
            {
                return Unauthorized(new
                {
                    message = "Invalid Username or Password."
                });
            }

            bool passwordValid = _managerAuthService.VerifyPassword(
                manager.PasswordHash,
                request.Password);

            if (!passwordValid)
            {
                return Unauthorized(new
                {
                    message = "Invalid Username or Password."
                });
            }

            return Ok(new
            {
                managerId = manager.ManagerId,
                username = manager.Username,
                name = manager.Name,
                managerLevel = manager.ManagerLevel,
                managerKey = manager.ManagerKey
            });
        }


        

        // GET: /managers/{managerId}/employees
        [HttpGet("{managerId}/employees")]
        public async Task<IActionResult> GetManagerEmployees(int managerId)
        {
            var manager = await _managerAuthService
                .GetManagerByIdAsync(managerId);

            if (manager == null)
            {
                return NotFound(new
                {
                    message = "Manager not found."
                });
            }

            var employees = await _managerAuthService
                .GetManagerEmployeesAsync(managerId);

            return Ok(employees);
        }
    }

    public class ManagerLoginRequest
    {
        public string Username { get; set; } = "";

        public string Password { get; set; } = "";
    }
}