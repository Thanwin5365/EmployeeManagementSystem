using EmployeeJoiningAPI.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace EmployeeJoiningAPI.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly string _connectionString;

        public EmployeeRepository(IConfiguration configuration)
        {
            _connectionString =
                configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException(
                    "Connection string 'DefaultConnection' not found.");
        }


        public async Task<bool> ValidateUser(
            string username,
            string hashedPassword)
        {
            using var conn =
                new SqlConnection(_connectionString);

            await conn.OpenAsync();

            using var cmd = new SqlCommand(
                "dbo.sp_ValidateUser",
                conn);

            cmd.CommandType =
                CommandType.StoredProcedure;

            cmd.Parameters.Add(
                "@Username",
                SqlDbType.NVarChar,
                100).Value = username;

            cmd.Parameters.Add(
                "@Password",
                SqlDbType.NVarChar,
                500).Value = hashedPassword;

            var result = await cmd.ExecuteScalarAsync();

            return result != null &&
                   result != DBNull.Value &&
                   Convert.ToBoolean(result);
        }



        public async Task<Employee?> GetEmployeeForLoginAsync(string username)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            using var cmd = new SqlCommand("dbo.sp_GetEmployeeForLogin", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@Username", SqlDbType.NVarChar, 100).Value = username;

            using var reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new Employee
                {
                    EmployeeId = reader.GetInt32(reader.GetOrdinal("EmployeeId")),
                    Username = reader.GetString(reader.GetOrdinal("Username")),
                    Name = reader.GetString(reader.GetOrdinal("Name")),
                    Department = reader.GetString(reader.GetOrdinal("Department")),

                    Designation = reader["Designation"] == DBNull.Value
                        ? null
                        : reader["Designation"].ToString(),

                    Email = reader["Email"] == DBNull.Value
                        ? null
                        : reader["Email"].ToString(),

                    Phone = reader["Phone"] == DBNull.Value
                        ? null
                        : reader["Phone"].ToString(),

                    Location = reader["Location"] == DBNull.Value
                        ? null
                        : reader["Location"].ToString()
                };
            }

            return null;
        }


        public async Task<List<Employee>> GetAllEmployeesAsync()
{
    var employees = new List<Employee>();

    using var conn =
        new SqlConnection(_connectionString);

    await conn.OpenAsync();

    using var cmd = new SqlCommand(
        "dbo.sp_GetAllEmployees",
        conn);

    cmd.CommandType =
        CommandType.StoredProcedure;

    using var reader =
        await cmd.ExecuteReaderAsync();

    while (await reader.ReadAsync())
    {
        var employee = new Employee
        {
            EmployeeId = reader.GetInt32(
                reader.GetOrdinal("EmployeeId")),

            Username = reader.GetString(
                reader.GetOrdinal("Username")),

            Name = reader.GetString(
                reader.GetOrdinal("Name")),

            Department = reader.GetString(
                reader.GetOrdinal("Department")),          
        };

        employees.Add(employee);
    }

    return employees;
}

        public async Task<Employee?> GetEmployeeByIdAsync(int employeeId)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            using var cmd = new SqlCommand(
                "dbo.sp_GetEmployeeById",
                conn);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@EmployeeId", SqlDbType.Int).Value = employeeId;

            using var reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new Employee
                {
                    EmployeeId = reader.GetInt32(reader.GetOrdinal("EmployeeId")),
                    Username = reader.GetString(reader.GetOrdinal("Username")),
                    Name = reader.GetString(reader.GetOrdinal("Name")),
                    Department = reader.GetString(reader.GetOrdinal("Department")),

                    Designation = reader["Designation"] == DBNull.Value
                        ? null
                        : reader["Designation"].ToString(),

                    Email = reader["Email"] == DBNull.Value
                        ? null
                        : reader["Email"].ToString(),

                    Phone = reader["Phone"] == DBNull.Value
                        ? null
                        : reader["Phone"].ToString(),

                    Location = reader["Location"] == DBNull.Value
                        ? null
                        : reader["Location"].ToString()
                };
            }

            return null;
        }


        public async Task<string?> GetPasswordHashAsync(string username)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            using var cmd = new SqlCommand(
                @"SELECT PasswordHash
          FROM dbo.Employees
          WHERE Username = @Username
            AND IsActive = 1",
                conn);

            cmd.Parameters.Add(
                "@Username",
                SqlDbType.NVarChar,
                100).Value = username;

           

            var result = await cmd.ExecuteScalarAsync();

            return result == null || result == DBNull.Value
                ? null
                : Convert.ToString(result);
        }

    }
}