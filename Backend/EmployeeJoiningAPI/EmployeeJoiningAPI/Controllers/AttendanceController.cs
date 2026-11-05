
using EmployeeJoiningAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeJoiningAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AttendanceController : ControllerBase
    {
        private readonly IAttendanceService _attendanceService;

        public AttendanceController(
            IAttendanceService attendanceService)
        {
            _attendanceService = attendanceService;
        }

        [HttpGet("{employeeId:int}")]
        public async Task<IActionResult> GetTodayAttendance(int employeeId)
        {
            try
            {
                var attendance =
                    await _attendanceService.GetTodayAttendanceAsync(employeeId);

                return Ok(attendance);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("check-in/{employeeId:int}")]
        public async Task<IActionResult> PunchIn(int employeeId)
        {
            try
            {
                var attendance =
                    await _attendanceService.PunchInAsync(employeeId);

                return Ok(new
                {
                    message = "Check-in successful.",
                    attendance
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpPost("check-out/{employeeId:int}")]
        public async Task<IActionResult> PunchOut(int employeeId)
        {
            try
            {
                var attendance =
                    await _attendanceService.PunchOutAsync(employeeId);

                return Ok(new
                {
                    message = "Check-out successful.",
                    attendance
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }
    }
}
