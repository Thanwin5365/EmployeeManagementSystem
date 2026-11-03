
using EmployeeJoiningAPI.Models;

namespace EmployeeJoiningAPI.Services
{
    public interface IAttendanceService
    {
        Task<AttendanceResponse?> GetTodayAttendanceAsync(int employeeId);

        Task<AttendanceResponse?> PunchInAsync(int employeeId);

        Task<AttendanceResponse?> PunchOutAsync(int employeeId);
    }
}
