
using EmployeeJoiningAPI.Models;
using Npgsql;

namespace EmployeeJoiningAPI.Repositories
{
    public class AttendanceRepository
    {
        private readonly string _connectionString;

        public AttendanceRepository(IConfiguration configuration)
        {
            _connectionString =
                configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException(
                    "DefaultConnection is not configured.");
        }

        public async Task<AttendanceResponse?> GetTodayAttendanceAsync(
            int employeeId)
        {
            await using var connection =
                new NpgsqlConnection(_connectionString);

            await connection.OpenAsync();

            await using var command = new NpgsqlCommand(
                "SELECT * FROM get_today_attendance(@p_employee_id)",
                connection);

            command.Parameters.AddWithValue(
                "p_employee_id", employeeId);

            await using var reader = await command.ExecuteReaderAsync();

            if (!await reader.ReadAsync())
                return null;

            return MapAttendance(reader);
        }

        public async Task<AttendanceResponse?> PunchInAsync(int employeeId)
        {
            await ExecutePunchFunctionAsync("punch_in", employeeId);

            return await GetTodayAttendanceAsync(employeeId);
        }

        public async Task<AttendanceResponse?> PunchOutAsync(int employeeId)
        {
            await ExecutePunchFunctionAsync("punch_out", employeeId);

            return await GetTodayAttendanceAsync(employeeId);
        }

        private async Task ExecutePunchFunctionAsync(
            string functionName,
            int employeeId)
        {
            await using var connection =
                new NpgsqlConnection(_connectionString);

            await connection.OpenAsync();

            // Function names are internal constants, not user input.
            await using var command = new NpgsqlCommand(
                $"SELECT * FROM {functionName}(@p_employee_id)",
                connection);

            command.Parameters.AddWithValue(
                "p_employee_id", employeeId);

            await using var reader = await command.ExecuteReaderAsync();

            if (!await reader.ReadAsync())
            {
                throw new InvalidOperationException(
                    "The punch function did not return a punch record.");
            }
        }

        public async Task<AttendanceResponse?> UpdateDailyAttendanceAsync(
            int employeeId)
        {
            await using var connection =
                new NpgsqlConnection(_connectionString);

            await connection.OpenAsync();

            await using var command = new NpgsqlCommand(
                "SELECT * FROM update_daily_attendance(@p_employee_id, TRUE)",
                connection);

            command.Parameters.AddWithValue(
                "p_employee_id", employeeId);

            await using var reader = await command.ExecuteReaderAsync();

            if (!await reader.ReadAsync())
                return null;

            return MapAttendance(reader);
        }

        private static AttendanceResponse MapAttendance(
            NpgsqlDataReader reader)
        {
            return new AttendanceResponse
            {
                AttendanceId = reader.GetInt32(
                    reader.GetOrdinal("AttendanceId")),

                EmployeeId = reader.GetInt32(
                    reader.GetOrdinal("EmployeeId")),

                AttendanceDate = reader.GetDateTime(
                    reader.GetOrdinal("AttendanceDate")),

                FinalCheckIn = reader.IsDBNull(
                    reader.GetOrdinal("FinalCheckIn"))
                    ? null
                    : reader.GetDateTime(
                        reader.GetOrdinal("FinalCheckIn")),

                FinalCheckOut = reader.IsDBNull(
                    reader.GetOrdinal("FinalCheckOut"))
                    ? null
                    : reader.GetDateTime(
                        reader.GetOrdinal("FinalCheckOut")),

                Status = reader.GetString(
                    reader.GetOrdinal("Status")),

                TotalWorkingHours = reader.GetDecimal(
                    reader.GetOrdinal("TotalWorkingHours"))
            };
        }
    }
}
