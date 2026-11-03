
using EmployeeJoiningAPI.Models;
using Microsoft.Data.SqlClient;
using System.Data;

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

            using var conn =
                new SqlConnection(_connectionString);

            await conn.OpenAsync();

            using var cmd = new SqlCommand(
                "SELECT * FROM dbo.Managers",
                conn);

            cmd.CommandType = CommandType.Text;

            using var reader =
                await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var manager = new Manager
                {
                    ManagerId = reader.GetInt32(
                        reader.GetOrdinal("ManagerId")),

                    Username = reader.GetString(
                        reader.GetOrdinal("Username")),

                    Name = reader.GetString(
                        reader.GetOrdinal("Name")),

                    ManagerLevel = reader.GetInt32(
                        reader.GetOrdinal("ManagerLevel")),

                    ManagerKey = reader.GetString(
                        reader.GetOrdinal("ManagerKey")),

                    PasswordHash = reader.GetString(
                        reader.GetOrdinal("PasswordHash"))
                };

                managers.Add(manager);
            }

            return managers;
        }

        
        public async Task<Manager?> GetManagerByIdAsync(int managerId)
        {
            using var conn = new SqlConnection(_connectionString);

            await conn.OpenAsync();

            using var cmd = new SqlCommand(
                "SELECT * FROM dbo.Managers WHERE ManagerId = @ManagerId",
                conn);

            cmd.CommandType = CommandType.Text;

            cmd.Parameters.Add(
                "@ManagerId",
                SqlDbType.Int).Value = managerId;

            using var reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new Manager
                {
                    ManagerId = reader.GetInt32(
                        reader.GetOrdinal("ManagerId")),

                    Username = reader.GetString(
                        reader.GetOrdinal("Username")),

                    Name = reader.GetString(
                        reader.GetOrdinal("Name")),

                    ManagerLevel = reader.GetInt32(
                        reader.GetOrdinal("ManagerLevel")),

                    ManagerKey = reader.GetString(
                        reader.GetOrdinal("ManagerKey")),

                    PasswordHash = reader.GetString(
                        reader.GetOrdinal("PasswordHash"))
                };
            }

            return null;
        }


        public async Task<List<Employee>> GetManagerEmployeesAsync(
            int managerId)
        {
            var employees = new List<Employee>();

            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            string? managerKey = null;

            using (var managerCmd = new SqlCommand(
                "SELECT ManagerKey FROM dbo.Managers WHERE ManagerId = @ManagerId",
                conn))
            {
                managerCmd.Parameters.Add(
                    "@ManagerId",
                    SqlDbType.Int).Value = managerId;

                var result = await managerCmd.ExecuteScalarAsync();

                if (result == null || result == DBNull.Value)
                {
                    return employees;
                }

                managerKey = Convert.ToString(result);
            }

            using var cmd = new SqlCommand(
                "dbo.GetEmployeesByManager",
                conn);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(
                "@ManagerKey",
                SqlDbType.NVarChar,
                100).Value = managerKey!;

            using var reader = await cmd.ExecuteReaderAsync();

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
                        reader.GetOrdinal("Department"))
                };

                employees.Add(employee);
            }

            return employees;
        }

        
        



        public async Task<Manager?> GetManagerForLoginAsync( 
            string username)
        {
            using var conn =
                new SqlConnection(_connectionString);

            await conn.OpenAsync();

            using var cmd = new SqlCommand(
                "dbo.sp_GetManagerForLogin",
                conn);

            cmd.CommandType =
                CommandType.StoredProcedure;

            cmd.Parameters.Add(
                "@Username",
                SqlDbType.NVarChar,
                100).Value = username;

            using var reader =
                await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new Manager
                {
                    ManagerId = reader.GetInt32(
                        reader.GetOrdinal("ManagerId")),

                    Username = reader.GetString(
                        reader.GetOrdinal("Username")),

                    Name = reader.GetString(
                        reader.GetOrdinal("Name")),

                    ManagerLevel = reader.GetInt32(
                        reader.GetOrdinal("ManagerLevel")),

                    ManagerKey = reader.GetString(
                        reader.GetOrdinal("ManagerKey")),
                    
                   

                    PasswordHash = reader.GetString(
                        reader.GetOrdinal("PasswordHash")),
                    
                   

                    


                };
            }

            return null;
        }
    }
}
