
using EmployeeJoiningAPI.Models;
using Npgsql;
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
            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            await using var cmd = new NpgsqlCommand(
                "SELECT validate_user(@p_username, @p_password)", conn);

            cmd.Parameters.AddWithValue("p_username", username);
            cmd.Parameters.AddWithValue("p_password", hashedPassword);

            var result = await cmd.ExecuteScalarAsync();

            return result != null &&
                   result != DBNull.Value &&
                   Convert.ToBoolean(result);
        }

        public async Task<Employee?> GetEmployeeForLoginAsync(string username)
        {
            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            await using var cmd = new NpgsqlCommand(
                "SELECT * FROM get_employee_for_login(@p_username)", conn);

            cmd.Parameters.AddWithValue("p_username", username);

            await using var reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return MapEmployee(reader);
            }

            return null;
        }

        public async Task<List<Employee>> GetAllEmployeesAsync()
        {
            var employees = new List<Employee>();

            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            await using var cmd = new NpgsqlCommand(
                "SELECT * FROM get_all_employees()", conn);

            await using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                employees.Add(MapEmployee(reader));
            }

            return employees;
        }

        public async Task<Employee?> GetEmployeeByIdAsync(int employeeId)
        {
            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            await using var cmd = new NpgsqlCommand(
                "SELECT * FROM get_employee_by_id(@p_employee_id)", conn);

            cmd.Parameters.AddWithValue("p_employee_id", employeeId);

            await using var reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return MapEmployee(reader);
            }

            return null;
        }

        public async Task<string?> GetPasswordHashAsync(string username)
        {
            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            await using var cmd = new NpgsqlCommand(
                "SELECT get_employee_password_hash(@p_username)", conn);

            cmd.Parameters.AddWithValue("p_username", username);

            var result = await cmd.ExecuteScalarAsync();

            return result == null || result == DBNull.Value
                ? null
                : Convert.ToString(result);
        }

        private static Employee MapEmployee(NpgsqlDataReader reader)
        {
            return new Employee
            {
                EmployeeId = reader.GetInt32(reader.GetOrdinal("EmployeeId")),

                Username = reader["Username"] == DBNull.Value
                    ? null
                    : reader["Username"].ToString(),

                Name = reader["Name"].ToString()!,
                Department = reader["Department"].ToString()!,

                Designation = reader["Designation"] == DBNull.Value
                    ? null : reader["Designation"].ToString(),

                Email = reader["Email"] == DBNull.Value
                    ? null : reader["Email"].ToString(),

                Phone = reader["Phone"] == DBNull.Value
                    ? null : reader["Phone"].ToString(),

                Location = reader["Location"] == DBNull.Value
                    ? null : reader["Location"].ToString()
            };
        }
    }
}
