
using System.Data;
using Microsoft.Data.SqlClient;
using EmployeeJoiningAPI.Models;

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
            using var connection = new SqlConnection(_connectionString);

            using var command = new SqlCommand(
                "dbo.sp_GetTodayAttendance", connection);

            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.Add(
                "@EmployeeId", SqlDbType.Int).Value = employeeId;

            await connection.OpenAsync();

            using var reader = await command.ExecuteReaderAsync();

            if (!await reader.ReadAsync())
                return null;

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
    reader.GetOrdinal("TotalWorkingHours")
),
            };
        }


        
public async Task<AttendanceResponse?> PunchInAsync(int employeeId)
{
    await ExecutePunchProcedureAsync(
        "dbo.sp_PunchIn", employeeId);

    return await GetTodayAttendanceAsync(employeeId);
}

public async Task<AttendanceResponse?> PunchOutAsync(int employeeId)
{
    await ExecutePunchProcedureAsync(
        "dbo.sp_PunchOut", employeeId);

    return await GetTodayAttendanceAsync(employeeId);
}

        private async Task ExecutePunchProcedureAsync(
            string procedureName,
            int employeeId)
        {
            using var connection = new SqlConnection(_connectionString);

            using var command = new SqlCommand(
                procedureName, connection);

            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.Add(
                "@EmployeeId", SqlDbType.Int).Value = employeeId;

            await connection.OpenAsync();

            using var reader = await command.ExecuteReaderAsync();

            if (!await reader.ReadAsync())
            {
                throw new InvalidOperationException(
                    "The punch procedure did not return a punch record.");
            }
        }


        public async Task<AttendanceResponse?> UpdateDailyAttendanceAsync(
            int employeeId)
        {
            using var connection = new SqlConnection(_connectionString);

            using var command = new SqlCommand(
                "dbo.sp_UpdateDailyAttendance", connection);

            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.Add(
                "@EmployeeId", SqlDbType.Int).Value = employeeId;

            await connection.OpenAsync();

            using var reader = await command.ExecuteReaderAsync();

            if (!await reader.ReadAsync())
                return null;

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
                   reader.GetOrdinal("TotalWorkingHours")
),
            };
        }

        
        
    }
}
