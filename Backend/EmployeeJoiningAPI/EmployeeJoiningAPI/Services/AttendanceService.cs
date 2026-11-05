
using EmployeeJoiningAPI.Models;
using EmployeeJoiningAPI.Repositories;

namespace EmployeeJoiningAPI.Services
{
    public class AttendanceService : IAttendanceService
    {
        private readonly AttendanceRepository _repository;

        public AttendanceService(AttendanceRepository repository)
        {
            _repository = repository;
        }

        public async Task<AttendanceResponse?> GetTodayAttendanceAsync(
            int employeeId)
        {
            ValidateEmployeeId(employeeId);

            return await _repository.GetTodayAttendanceAsync(employeeId);
        }

        public async Task<AttendanceResponse?> PunchInAsync(
            int employeeId)
        {
            ValidateEmployeeId(employeeId);

            return await _repository.PunchInAsync(employeeId);
        }

        public async Task<AttendanceResponse?> PunchOutAsync(
            int employeeId)
        {
            ValidateEmployeeId(employeeId);

            return await _repository.PunchOutAsync(employeeId);
        }

        private static void ValidateEmployeeId(int employeeId)
        {
            if (employeeId <= 0)
            {
                throw new ArgumentException("Invalid employee ID.");
            }
        }
    }
}
