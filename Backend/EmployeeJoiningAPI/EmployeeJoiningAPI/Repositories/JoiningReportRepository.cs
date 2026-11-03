
using System.Data;
using Microsoft.Data.SqlClient;
using EmployeeJoiningAPI.Models;

namespace EmployeeJoiningAPI.Repositories
{
    public class JoiningReportRepository : IJoiningReportRepository
    {
        private readonly string _connectionString;

        public JoiningReportRepository(
            IConfiguration configuration)
        {
            _connectionString =
                configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException(
                    "Connection string 'DefaultConnection' was not found.");
        }

        private SqlConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }

        
public async Task<List<JoiningReportApprovalHistory>>
    GetApprovalHistoryAsync(int id)
        {
            var history = new List<JoiningReportApprovalHistory>();

            await using var connection = CreateConnection();

            await using var command = new SqlCommand(
                "dbo.sp_GetJoiningReportApprovalHistory",
                connection);

            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.Add("@Id", SqlDbType.Int).Value = id;

            await connection.OpenAsync();

            await using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                history.Add(new JoiningReportApprovalHistory
                {
                    ApprovalHistoryId = reader.GetInt32(
                        reader.GetOrdinal("ApprovalHistoryId")),

                    Id = reader.GetInt32(reader.GetOrdinal("Id")),

                    ManagerId = reader.GetInt32(
                        reader.GetOrdinal("ManagerId")),

                    ManagerName = reader.IsDBNull(
                        reader.GetOrdinal("ManagerName"))
                        ? null
                        : reader.GetString(reader.GetOrdinal("ManagerName")),

                    ManagerLevel = reader.GetString(
                        reader.GetOrdinal("ManagerLevel")),

                    ApprovalLevel = reader.GetInt32(
                        reader.GetOrdinal("ApprovalLevel")),

                    ApprovalStatus = reader.GetString(
                        reader.GetOrdinal("ApprovalStatus")),

                    RejectionReason = reader.IsDBNull(
                        reader.GetOrdinal("RejectionReason"))
                        ? null
                        : reader.GetString(reader.GetOrdinal("RejectionReason")),

                    ModifiedBy = reader.IsDBNull(
                        reader.GetOrdinal("ModifiedBy"))
                        ? null
                        : reader.GetString(reader.GetOrdinal("ModifiedBy")),

                    ModifiedDate = reader.GetDateTime(
                        reader.GetOrdinal("ModifiedDate"))
                });
            }

            return history;
        }

        
public async Task<List<JoiningReport>> GetPendingLevel2ReportsAsync(
    int level2ManagerId)
        {
            var reports = new List<JoiningReport>();

            await using var connection = CreateConnection();

            await using var command = new SqlCommand(
                "dbo.sp_GetPendingLevel2JoiningReports",
                connection);

            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.Add("@Level2ManagerId", SqlDbType.Int)
                .Value = level2ManagerId;

            await connection.OpenAsync();

            await using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                reports.Add(new JoiningReport
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    EmployeeId = reader.GetInt32(reader.GetOrdinal("EmployeeId")),
                    EmployeeName = reader.GetString(reader.GetOrdinal("EmployeeName")),
                    Designation = reader.GetString(reader.GetOrdinal("Designation")),
                    Department = reader.GetString(reader.GetOrdinal("Department")),
                    JoinedDutyOn = reader.GetDateTime(reader.GetOrdinal("JoinedDutyOn")),
                    JoiningType = reader.GetString(reader.GetOrdinal("JoiningType")),
                    LeaveAppliedTill = reader.IsDBNull(reader.GetOrdinal("LeaveAppliedTill"))
                        ? null
                        : reader.GetDateTime(reader.GetOrdinal("LeaveAppliedTill")),
                    DelayReason = reader.IsDBNull(reader.GetOrdinal("DelayReason"))
                        ? null
                        : reader.GetString(reader.GetOrdinal("DelayReason")),
                    FormDate = reader.GetDateTime(reader.GetOrdinal("FormDate")),
                    Signature = reader.GetString(reader.GetOrdinal("Signature")),
                    CreatedDate = reader.GetDateTime(reader.GetOrdinal("CreatedDate")),
                    ApprovalStatus = reader.GetString(reader.GetOrdinal("ApprovalStatus")),
                    Level1ManagerId = reader.IsDBNull(reader.GetOrdinal("Level1ManagerId"))
                        ? null
                        : reader.GetInt32(reader.GetOrdinal("Level1ManagerId")),
                    Level1ApprovedDate = reader.IsDBNull(reader.GetOrdinal("Level1ApprovedDate"))
                        ? null
                        : reader.GetDateTime(reader.GetOrdinal("Level1ApprovedDate")),
                    Level1RejectionReason = reader.IsDBNull(reader.GetOrdinal("Level1RejectionReason"))
                        ? null
                        : reader.GetString(reader.GetOrdinal("Level1RejectionReason")),
                    Level2ManagerId = reader.IsDBNull(reader.GetOrdinal("Level2ManagerId"))
                        ? null
                        : reader.GetInt32(reader.GetOrdinal("Level2ManagerId")),
                    Level2ApprovedDate = reader.IsDBNull(reader.GetOrdinal("Level2ApprovedDate"))
                        ? null
                        : reader.GetDateTime(reader.GetOrdinal("Level2ApprovedDate")),
                    Level2RejectionReason = reader.IsDBNull(reader.GetOrdinal("Level2RejectionReason"))
                        ? null
                        : reader.GetString(reader.GetOrdinal("Level2RejectionReason")),
                    HrManagerId = reader.IsDBNull(reader.GetOrdinal("HrManagerId"))
                        ? null
                        : reader.GetInt32(reader.GetOrdinal("HrManagerId")),
                    HrApprovedDate = reader.IsDBNull(reader.GetOrdinal("HrApprovedDate"))
                        ? null
                        : reader.GetDateTime(reader.GetOrdinal("HrApprovedDate")),
                    HrRejectionReason = reader.IsDBNull(reader.GetOrdinal("HrRejectionReason"))
                        ? null
                        : reader.GetString(reader.GetOrdinal("HrRejectionReason"))
                });
            }

            return reports;
        }

        
public async Task<List<JoiningReport>> GetPendingHrReportsAsync()
        {
            var reports = new List<JoiningReport>();

            await using var connection = CreateConnection();

            await using var command = new SqlCommand(
                "dbo.sp_GetPendingHrJoiningReports",
                connection);

            command.CommandType = CommandType.StoredProcedure;

            await connection.OpenAsync();

            await using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                reports.Add(new JoiningReport
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    EmployeeId = reader.GetInt32(reader.GetOrdinal("EmployeeId")),
                    EmployeeName = reader.GetString(reader.GetOrdinal("EmployeeName")),
                    Designation = reader.GetString(reader.GetOrdinal("Designation")),
                    Department = reader.GetString(reader.GetOrdinal("Department")),
                    JoinedDutyOn = reader.GetDateTime(reader.GetOrdinal("JoinedDutyOn")),
                    JoiningType = reader.GetString(reader.GetOrdinal("JoiningType")),

                    LeaveAppliedTill = reader.IsDBNull(reader.GetOrdinal("LeaveAppliedTill"))
                        ? null
                        : reader.GetDateTime(reader.GetOrdinal("LeaveAppliedTill")),

                    DelayReason = reader.IsDBNull(reader.GetOrdinal("DelayReason"))
                        ? null
                        : reader.GetString(reader.GetOrdinal("DelayReason")),

                    FormDate = reader.GetDateTime(reader.GetOrdinal("FormDate")),
                    Signature = reader.GetString(reader.GetOrdinal("Signature")),
                    CreatedDate = reader.GetDateTime(reader.GetOrdinal("CreatedDate")),
                    ApprovalStatus = reader.GetString(reader.GetOrdinal("ApprovalStatus")),

                    Level1ManagerId = reader.IsDBNull(reader.GetOrdinal("Level1ManagerId"))
                        ? null
                        : reader.GetInt32(reader.GetOrdinal("Level1ManagerId")),

                    Level1ApprovedDate = reader.IsDBNull(reader.GetOrdinal("Level1ApprovedDate"))
                        ? null
                        : reader.GetDateTime(reader.GetOrdinal("Level1ApprovedDate")),

                    Level1RejectionReason = reader.IsDBNull(reader.GetOrdinal("Level1RejectionReason"))
                        ? null
                        : reader.GetString(reader.GetOrdinal("Level1RejectionReason")),

                    Level2ManagerId = reader.IsDBNull(reader.GetOrdinal("Level2ManagerId"))
                        ? null
                        : reader.GetInt32(reader.GetOrdinal("Level2ManagerId")),

                    Level2ApprovedDate = reader.IsDBNull(reader.GetOrdinal("Level2ApprovedDate"))
                        ? null
                        : reader.GetDateTime(reader.GetOrdinal("Level2ApprovedDate")),

                    Level2RejectionReason = reader.IsDBNull(reader.GetOrdinal("Level2RejectionReason"))
                        ? null
                        : reader.GetString(reader.GetOrdinal("Level2RejectionReason")),

                    HrManagerId = reader.IsDBNull(reader.GetOrdinal("HrManagerId"))
                        ? null
                        : reader.GetInt32(reader.GetOrdinal("HrManagerId")),

                    HrApprovedDate = reader.IsDBNull(reader.GetOrdinal("HrApprovedDate"))
                        ? null
                        : reader.GetDateTime(reader.GetOrdinal("HrApprovedDate")),

                    HrRejectionReason = reader.IsDBNull(reader.GetOrdinal("HrRejectionReason"))
                        ? null
                        : reader.GetString(reader.GetOrdinal("HrRejectionReason"))
                });
            }

            return reports;
        }

        
public async Task<List<JoiningReport>> GetAllReportsAsync()
        {
            var reports = new List<JoiningReport>();

            await using var connection = CreateConnection();

            await using var command = new SqlCommand(
                "dbo.sp_GetAllJoiningReports",
                connection);

            command.CommandType = CommandType.StoredProcedure;

            await connection.OpenAsync();



            await using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                reports.Add(new JoiningReport
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    EmployeeId = reader.GetInt32(reader.GetOrdinal("EmployeeId")),
                    EmployeeName = reader.GetString(reader.GetOrdinal("EmployeeName")),
                    Designation = reader.GetString(reader.GetOrdinal("Designation")),
                    Department = reader.GetString(reader.GetOrdinal("Department")),
                    JoinedDutyOn = reader.GetDateTime(reader.GetOrdinal("JoinedDutyOn")),
                    JoiningType = reader.GetString(reader.GetOrdinal("JoiningType")),
                    LeaveAppliedTill = reader.IsDBNull(reader.GetOrdinal("LeaveAppliedTill"))
                        ? null
                        : reader.GetDateTime(reader.GetOrdinal("LeaveAppliedTill")),
                    DelayReason = reader.IsDBNull(reader.GetOrdinal("DelayReason"))
                        ? null
                        : reader.GetString(reader.GetOrdinal("DelayReason")),
                    FormDate = reader.GetDateTime(reader.GetOrdinal("FormDate")),
                    Signature = reader.GetString(reader.GetOrdinal("Signature")),
                    CreatedDate = reader.GetDateTime(reader.GetOrdinal("CreatedDate")),
                    ApprovalStatus = reader.GetString(reader.GetOrdinal("ApprovalStatus")),
                    Level1ManagerId = reader.IsDBNull(reader.GetOrdinal("Level1ManagerId"))
                        ? null
                        : reader.GetInt32(reader.GetOrdinal("Level1ManagerId")),
                    Level1ApprovedDate = reader.IsDBNull(reader.GetOrdinal("Level1ApprovedDate"))
                        ? null
                        : reader.GetDateTime(reader.GetOrdinal("Level1ApprovedDate")),
                    Level1RejectionReason = reader.IsDBNull(reader.GetOrdinal("Level1RejectionReason"))
                        ? null
                        : reader.GetString(reader.GetOrdinal("Level1RejectionReason")),
                    Level2ManagerId = reader.IsDBNull(reader.GetOrdinal("Level2ManagerId"))
                        ? null
                        : reader.GetInt32(reader.GetOrdinal("Level2ManagerId")),
                    Level2ApprovedDate = reader.IsDBNull(reader.GetOrdinal("Level2ApprovedDate"))
                        ? null
                        : reader.GetDateTime(reader.GetOrdinal("Level2ApprovedDate")),
                    Level2RejectionReason = reader.IsDBNull(reader.GetOrdinal("Level2RejectionReason"))
                        ? null
                        : reader.GetString(reader.GetOrdinal("Level2RejectionReason")),
                    HrManagerId = reader.IsDBNull(reader.GetOrdinal("HrManagerId"))
                        ? null
                        : reader.GetInt32(reader.GetOrdinal("HrManagerId")),
                    HrApprovedDate = reader.IsDBNull(reader.GetOrdinal("HrApprovedDate"))
                        ? null
                        : reader.GetDateTime(reader.GetOrdinal("HrApprovedDate")),
                    HrRejectionReason = reader.IsDBNull(reader.GetOrdinal("HrRejectionReason"))
                        ? null
                        : reader.GetString(reader.GetOrdinal("HrRejectionReason"))
                });
            }

            return reports;
        }

        
public async Task<JoiningReport?> GetReportByIdAsync(int id)
        {
            JoiningReport? report = null;

            await using var connection = CreateConnection();

            await using var command = new SqlCommand(
                "dbo.sp_GetJoiningReportById",
                connection);

            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.Add("@Id", SqlDbType.Int).Value = id;

            await connection.OpenAsync();

            await using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                report = new JoiningReport
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    EmployeeId = reader.GetInt32(reader.GetOrdinal("EmployeeId")),
                    EmployeeName = reader.GetString(reader.GetOrdinal("EmployeeName")),
                    Designation = reader.GetString(reader.GetOrdinal("Designation")),
                    Department = reader.GetString(reader.GetOrdinal("Department")),
                    JoinedDutyOn = reader.GetDateTime(reader.GetOrdinal("JoinedDutyOn")),
                    JoiningType = reader.GetString(reader.GetOrdinal("JoiningType")),

                    LeaveAppliedTill = reader.IsDBNull(reader.GetOrdinal("LeaveAppliedTill"))
                        ? null
                        : reader.GetDateTime(reader.GetOrdinal("LeaveAppliedTill")),

                    DelayReason = reader.IsDBNull(reader.GetOrdinal("DelayReason"))
                        ? null
                        : reader.GetString(reader.GetOrdinal("DelayReason")),

                    FormDate = reader.GetDateTime(reader.GetOrdinal("FormDate")),
                    Signature = reader.GetString(reader.GetOrdinal("Signature")),
                    CreatedDate = reader.GetDateTime(reader.GetOrdinal("CreatedDate")),
                    ApprovalStatus = reader.GetString(reader.GetOrdinal("ApprovalStatus")),

                    Level1ManagerId = reader.IsDBNull(reader.GetOrdinal("Level1ManagerId"))
                        ? null
                        : reader.GetInt32(reader.GetOrdinal("Level1ManagerId")),

                    Level1ApprovedDate = reader.IsDBNull(reader.GetOrdinal("Level1ApprovedDate"))
                        ? null
                        : reader.GetDateTime(reader.GetOrdinal("Level1ApprovedDate")),

                    Level1RejectionReason = reader.IsDBNull(reader.GetOrdinal("Level1RejectionReason"))
                        ? null
                        : reader.GetString(reader.GetOrdinal("Level1RejectionReason")),

                    Level2ManagerId = reader.IsDBNull(reader.GetOrdinal("Level2ManagerId"))
                        ? null
                        : reader.GetInt32(reader.GetOrdinal("Level2ManagerId")),

                    Level2ApprovedDate = reader.IsDBNull(reader.GetOrdinal("Level2ApprovedDate"))
                        ? null
                        : reader.GetDateTime(reader.GetOrdinal("Level2ApprovedDate")),

                    Level2RejectionReason = reader.IsDBNull(reader.GetOrdinal("Level2RejectionReason"))
                        ? null
                        : reader.GetString(reader.GetOrdinal("Level2RejectionReason")),

                    HrManagerId = reader.IsDBNull(reader.GetOrdinal("HrManagerId"))
                        ? null
                        : reader.GetInt32(reader.GetOrdinal("HrManagerId")),

                    HrApprovedDate = reader.IsDBNull(reader.GetOrdinal("HrApprovedDate"))
                        ? null
                        : reader.GetDateTime(reader.GetOrdinal("HrApprovedDate")),

                    HrRejectionReason = reader.IsDBNull(reader.GetOrdinal("HrRejectionReason"))
                        ? null
                        : reader.GetString(reader.GetOrdinal("HrRejectionReason"))
                };
            }

            return report;
        }

        

        
public async Task<List<JoiningReport>> GetReportsByEmployeeAsync(
    int employeeId)
        {
            var reports = new List<JoiningReport>();

            await using var connection = CreateConnection();

            await using var command = new SqlCommand(
                "dbo.sp_GetJoiningReportsByEmployee",
                connection);

            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.Add("@EmployeeId", SqlDbType.Int)
                .Value = employeeId;

            await connection.OpenAsync();

            await using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                reports.Add(new JoiningReport
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    EmployeeId = reader.GetInt32(reader.GetOrdinal("EmployeeId")),
                    EmployeeName = reader.GetString(reader.GetOrdinal("EmployeeName")),
                    Designation = reader.GetString(reader.GetOrdinal("Designation")),
                    Department = reader.GetString(reader.GetOrdinal("Department")),
                    JoinedDutyOn = reader.GetDateTime(reader.GetOrdinal("JoinedDutyOn")),
                    JoiningType = reader.GetString(reader.GetOrdinal("JoiningType")),
                    LeaveAppliedTill = reader.IsDBNull(reader.GetOrdinal("LeaveAppliedTill"))
                        ? null
                        : reader.GetDateTime(reader.GetOrdinal("LeaveAppliedTill")),
                    DelayReason = reader.IsDBNull(reader.GetOrdinal("DelayReason"))
                        ? null
                        : reader.GetString(reader.GetOrdinal("DelayReason")),
                    FormDate = reader.GetDateTime(reader.GetOrdinal("FormDate")),
                    Signature = reader.GetString(reader.GetOrdinal("Signature")),
                    CreatedDate = reader.GetDateTime(reader.GetOrdinal("CreatedDate")),
                    ApprovalStatus = reader.GetString(reader.GetOrdinal("ApprovalStatus")),
                    Level1ManagerId = reader.IsDBNull(reader.GetOrdinal("Level1ManagerId"))
                        ? null
                        : reader.GetInt32(reader.GetOrdinal("Level1ManagerId")),
                    Level1ApprovedDate = reader.IsDBNull(reader.GetOrdinal("Level1ApprovedDate"))
                        ? null
                        : reader.GetDateTime(reader.GetOrdinal("Level1ApprovedDate")),
                    Level1RejectionReason = reader.IsDBNull(reader.GetOrdinal("Level1RejectionReason"))
                        ? null
                        : reader.GetString(reader.GetOrdinal("Level1RejectionReason")),
                    Level2ManagerId = reader.IsDBNull(reader.GetOrdinal("Level2ManagerId"))
                        ? null
                        : reader.GetInt32(reader.GetOrdinal("Level2ManagerId")),
                    Level2ApprovedDate = reader.IsDBNull(reader.GetOrdinal("Level2ApprovedDate"))
                        ? null
                        : reader.GetDateTime(reader.GetOrdinal("Level2ApprovedDate")),
                    Level2RejectionReason = reader.IsDBNull(reader.GetOrdinal("Level2RejectionReason"))
                        ? null
                        : reader.GetString(reader.GetOrdinal("Level2RejectionReason")),
                    HrManagerId = reader.IsDBNull(reader.GetOrdinal("HrManagerId"))
                        ? null
                        : reader.GetInt32(reader.GetOrdinal("HrManagerId")),
                    HrApprovedDate = reader.IsDBNull(reader.GetOrdinal("HrApprovedDate"))
                        ? null
                        : reader.GetDateTime(reader.GetOrdinal("HrApprovedDate")),
                    HrRejectionReason = reader.IsDBNull(reader.GetOrdinal("HrRejectionReason"))
                        ? null
                        : reader.GetString(reader.GetOrdinal("HrRejectionReason"))
                });
            }

            return reports;
        }

       
public async Task<JoiningReport> CreateReportAsync(
    JoiningReport joiningReport)
        {
            if (joiningReport == null)
            {
                throw new ArgumentNullException(nameof(joiningReport));
            }

            await using var connection = CreateConnection();

            await using var command = new SqlCommand(
                "dbo.sp_CreateJoiningReport",
                connection);

            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.Add("@EmployeeId", SqlDbType.Int)
                .Value = joiningReport.EmployeeId;

            command.Parameters.Add("@EmployeeName", SqlDbType.NVarChar, 150)
    .Value = joiningReport.EmployeeName;

            command.Parameters.Add("@Designation", SqlDbType.NVarChar, 150)
    .Value = joiningReport.Designation;

            command.Parameters.Add("@Department", SqlDbType.NVarChar, 150)
     .Value = joiningReport.Department;

            command.Parameters.Add("@JoinedDutyOn", SqlDbType.Date)
                .Value = joiningReport.JoinedDutyOn.Date;

            command.Parameters.Add("@JoiningType", SqlDbType.NVarChar, 50)
    .Value = joiningReport.JoiningType;

            command.Parameters.Add("@LeaveAppliedTill", SqlDbType.Date)
                .Value = (object?)joiningReport.LeaveAppliedTill?.Date
                    ?? DBNull.Value;

            command.Parameters.Add("@DelayReason", SqlDbType.NVarChar, 500)
    .Value = (object?)joiningReport.DelayReason ?? DBNull.Value;

            command.Parameters.Add("@FormDate", SqlDbType.Date)
                .Value = joiningReport.FormDate.Date;

            command.Parameters.Add("@Signature", SqlDbType.NVarChar, 150)
    .Value = joiningReport.Signature;

            await connection.OpenAsync();

            await using var reader = await command.ExecuteReaderAsync();

            if (!await reader.ReadAsync())
            {
                throw new InvalidOperationException(
                    "The joining report was created, but no record was returned.");
            }

            joiningReport.Id = reader.GetInt32(reader.GetOrdinal("Id"));
            joiningReport.CreatedDate =
                reader.GetDateTime(reader.GetOrdinal("CreatedDate"));
            joiningReport.ApprovalStatus =
                reader.GetString(reader.GetOrdinal("ApprovalStatus"));

            return joiningReport;
        }

        
// GET LEVEL 1 PENDING REPORTS
public async Task<List<JoiningReport>> GetPendingLevel1ReportsAsync()
        {
            var reports = new List<JoiningReport>();

            await using var connection = CreateConnection();
            await using var command = new SqlCommand(
                "dbo.sp_GetPendingLevel1JoiningReports", connection);

            command.CommandType = CommandType.StoredProcedure;

            await connection.OpenAsync();
            await using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                reports.Add(MapJoiningReport(reader));
            }

            return reports;
        }
        
       

        // LEVEL 1 APPROVE
        public async Task ApproveLevel1Async(int reportId, int? managerId)
        {
            if (managerId == null || managerId <= 0)
                throw new ArgumentException("Level 1 Manager ID is required.");

            await using var connection = CreateConnection();
            await using var command = new SqlCommand(
                "dbo.sp_ApproveLevel1JoiningReport", connection);

            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@ReportId", SqlDbType.Int).Value = reportId;
            command.Parameters.Add("@Level1ManagerId", SqlDbType.Int).Value = managerId.Value;

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }

        // LEVEL 1 REJECT
        public async Task RejectLevel1Async(int reportId, string reason)
        {
            if (string.IsNullOrWhiteSpace(reason))
                throw new ArgumentException("Rejection reason is required.");

            await using var connection = CreateConnection();
            await using var command = new SqlCommand(
                "dbo.sp_RejectLevel1JoiningReport", connection);

            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@ReportId", SqlDbType.Int).Value = reportId;
            command.Parameters.Add("@RejectionReason", SqlDbType.NVarChar, -1)
                .Value = reason.Trim();

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }

        // LEVEL 2 APPROVE
        public async Task ApproveLevel2Async(int reportId, int managerId)
        {
            await using var connection = CreateConnection();
            await using var command = new SqlCommand(
                "dbo.sp_ApproveLevel2JoiningReport", connection);

            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@ReportId", SqlDbType.Int).Value = reportId;
            command.Parameters.Add("@Level2ManagerId", SqlDbType.Int).Value = managerId;

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }

        // LEVEL 2 REJECT
        public async Task RejectLevel2Async(
            int reportId, int managerId, string reason)
        {
            await using var connection = CreateConnection();
            await using var command = new SqlCommand(
                "dbo.sp_RejectLevel2JoiningReport", connection);

            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@ReportId", SqlDbType.Int).Value = reportId;
            command.Parameters.Add("@Level2ManagerId", SqlDbType.Int).Value = managerId;
            command.Parameters.Add("@RejectionReason", SqlDbType.NVarChar, -1)
                .Value = reason.Trim();

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }

        // HR APPROVE
        public async Task ApproveHrAsync(int reportId, int hrManagerId)
        {
            await using var connection = CreateConnection();
            await using var command = new SqlCommand(
                "dbo.sp_ApproveHrJoiningReport", connection);

            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@ReportId", SqlDbType.Int).Value = reportId;
            command.Parameters.Add("@HrManagerId", SqlDbType.Int).Value = hrManagerId;

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }

        // HR REJECT
        public async Task RejectHrAsync(
            int reportId, int hrManagerId, string reason)
        {
            await using var connection = CreateConnection();
            await using var command = new SqlCommand(
                "dbo.sp_RejectHrJoiningReport", connection);

            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@ReportId", SqlDbType.Int).Value = reportId;
            command.Parameters.Add("@HrManagerId", SqlDbType.Int).Value = hrManagerId;
            command.Parameters.Add("@RejectionReason", SqlDbType.NVarChar, -1)
                .Value = reason.Trim();

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }

        
private static JoiningReport MapJoiningReport(SqlDataReader reader)
        {
            int Ord(string name) => reader.GetOrdinal(name);

            return new JoiningReport
            {
                Id = reader.GetInt32(Ord("Id")),
                EmployeeId = reader.GetInt32(Ord("EmployeeId")),
                EmployeeName = reader.GetString(Ord("EmployeeName")),
                Designation = reader.GetString(Ord("Designation")),
                Department = reader.GetString(Ord("Department")),
                JoinedDutyOn = reader.GetDateTime(Ord("JoinedDutyOn")),
                JoiningType = reader.GetString(Ord("JoiningType")),

                LeaveAppliedTill = reader.IsDBNull(Ord("LeaveAppliedTill"))
                    ? null : reader.GetDateTime(Ord("LeaveAppliedTill")),

                DelayReason = reader.IsDBNull(Ord("DelayReason"))
                    ? null : reader.GetString(Ord("DelayReason")),

                FormDate = reader.GetDateTime(Ord("FormDate")),
                Signature = reader.GetString(Ord("Signature")),
                CreatedDate = reader.GetDateTime(Ord("CreatedDate")),
                ApprovalStatus = reader.GetString(Ord("ApprovalStatus")),

                Level1ManagerId = reader.IsDBNull(Ord("Level1ManagerId"))
                    ? null : reader.GetInt32(Ord("Level1ManagerId")),

                Level1ApprovedDate = reader.IsDBNull(Ord("Level1ApprovedDate"))
                    ? null : reader.GetDateTime(Ord("Level1ApprovedDate")),

                Level1RejectionReason = reader.IsDBNull(Ord("Level1RejectionReason"))
                    ? null : reader.GetString(Ord("Level1RejectionReason")),

                Level2ManagerId = reader.IsDBNull(Ord("Level2ManagerId"))
                    ? null : reader.GetInt32(Ord("Level2ManagerId")),

                Level2ApprovedDate = reader.IsDBNull(Ord("Level2ApprovedDate"))
                    ? null : reader.GetDateTime(Ord("Level2ApprovedDate")),

                Level2RejectionReason = reader.IsDBNull(Ord("Level2RejectionReason"))
                    ? null : reader.GetString(Ord("Level2RejectionReason")),

                HrManagerId = reader.IsDBNull(Ord("HrManagerId"))
                    ? null : reader.GetInt32(Ord("HrManagerId")),

                HrApprovedDate = reader.IsDBNull(Ord("HrApprovedDate"))
                    ? null : reader.GetDateTime(Ord("HrApprovedDate")),

                HrRejectionReason = reader.IsDBNull(Ord("HrRejectionReason"))
                    ? null : reader.GetString(Ord("HrRejectionReason"))
            };
        }










        
    }
}
