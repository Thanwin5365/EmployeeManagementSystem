
using EmployeeJoiningAPI.Models;
using Npgsql;

namespace EmployeeJoiningAPI.Repositories
{
    public class ManagerRepository : IManagerRepository
    {
        private readonly string _connectionString;

        public ManagerRepository(IConfiguration configuration)
        {
            _connectionString =
                configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException(
                    "Connection string 'DefaultConnection' not found.");
        }

        public async Task<List<Manager>> GetAllManagersAsync()
        {
            var managers = new List<Manager>();

            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            await using var cmd = new NpgsqlCommand(
                "SELECT * FROM get_all_managers()", conn);

            await using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                managers.Add(MapManager(reader));
            }

            return managers;
        }

        public async Task<Manager?> GetManagerByIdAsync(int managerId)
        {
            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            await using var cmd = new NpgsqlCommand(
                "SELECT * FROM get_manager_by_id(@p_manager_id)", conn);

            cmd.Parameters.AddWithValue("p_manager_id", managerId);

            await using var reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return MapManager(reader);
            }

            return null;
        }

        public async Task<List<Employee>> GetManagerEmployeesAsync(int managerId)
        {
            var employees = new List<Employee>();

            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            string? managerKey;

            await using (var managerCmd = new NpgsqlCommand(
                "SELECT \"ManagerKey\" FROM \"Managers\" WHERE \"ManagerId\" = @p_manager_id",
                conn))
            {
                managerCmd.Parameters.AddWithValue("p_manager_id", managerId);

                var result = await managerCmd.ExecuteScalarAsync();

                if (result == null || result == DBNull.Value)
                {
                    return employees;
                }

                managerKey = Convert.ToString(result);
            }

            await using var cmd = new NpgsqlCommand(
                "SELECT * FROM get_employees_by_manager(@p_manager_key)", conn);

            cmd.Parameters.AddWithValue("p_manager_key", managerKey!);

            await using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                employees.Add(new Employee
                {
                    EmployeeId = reader.GetInt32(reader.GetOrdinal("EmployeeId")),
                    Username = reader["Username"] == DBNull.Value
                        ? null : reader["Username"].ToString(),
                    Name = reader["Name"].ToString()!,
                    Department = reader["Department"].ToString()!
                });
            }

            return employees;
        }

        public async Task<Manager?> GetManagerForLoginAsync(string username)
        {
            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            await using var cmd = new NpgsqlCommand(
                "SELECT * FROM get_manager_for_login(@p_username)", conn);

            cmd.Parameters.AddWithValue("p_username", username);

            await using var reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return MapManager(reader);
            }

            return null;
        }

        private static Manager MapManager(NpgsqlDataReader reader)
        {
            return new Manager
            {
                ManagerId = reader.GetInt32(reader.GetOrdinal("ManagerId")),

                Username = reader["Username"] == DBNull.Value
                    ? null : reader["Username"].ToString(),

                Name = reader["Name"].ToString()!,

                ManagerLevel = reader.GetInt32(
                    reader.GetOrdinal("ManagerLevel")),

                ManagerKey = reader["ManagerKey"] == DBNull.Value
                    ? null : reader["ManagerKey"].ToString(),

                PasswordHash = reader["PasswordHash"].ToString()!
            };
        }
    }
}
