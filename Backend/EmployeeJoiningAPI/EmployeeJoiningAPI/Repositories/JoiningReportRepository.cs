
using Npgsql;
using NpgsqlTypes;
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

        private NpgsqlConnection CreateConnection()
        {
            return new NpgsqlConnection(_connectionString);
        }

        
public async Task<List<JoiningReportApprovalHistory>>
    GetApprovalHistoryAsync(int id)
        {
            var history = new List<JoiningReportApprovalHistory>();

            await using var connection = CreateConnection();

            await using var command = new NpgsqlCommand(
                "SELECT * FROM get_joining_report_approval_history(@p_id)",
                connection);
            command.Parameters.Add("p_id", NpgsqlDbType.Integer).Value = id;

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

            await using var command = new NpgsqlCommand(
                "SELECT * FROM get_pending_level2_joining_reports(@p_level2_manager_id)",
                connection);
            command.Parameters.Add("p_level2_manager_id", NpgsqlDbType.Integer).Value = level2ManagerId;

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

            await using var command = new NpgsqlCommand(
                "SELECT * FROM get_pending_hr_joining_reports()",
                connection);
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

            await using var command = new NpgsqlCommand(
                "SELECT * FROM get_all_joining_reports()",
                connection);
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

            await using var command = new NpgsqlCommand(
                "SELECT * FROM get_joining_report_by_id(@p_id)",
                connection);
            command.Parameters.Add("p_id", NpgsqlDbType.Integer).Value = id;

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

            await using var command = new NpgsqlCommand(
                "SELECT * FROM get_joining_reports_by_employee(@p_employee_id)",
                connection);
            command.Parameters.Add("p_employee_id", NpgsqlDbType.Integer).Value = employeeId;

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

            await using var command = new NpgsqlCommand(
                "SELECT * FROM create_joining_report(@p_employee_id, @p_employee_name, @p_designation, @p_department, @p_joined_duty_on, @p_joining_type, @p_leave_applied_till, @p_delay_reason, @p_form_date, @p_signature)",
                connection);
            command.Parameters.Add("p_employee_id", NpgsqlDbType.Integer).Value = joiningReport.EmployeeId;

            command.Parameters.Add("p_employee_name", NpgsqlDbType.Text).Value = joiningReport.EmployeeName;

            command.Parameters.Add("p_designation", NpgsqlDbType.Text).Value = joiningReport.Designation;

            command.Parameters.Add("p_department", NpgsqlDbType.Text).Value = joiningReport.Department;

            command.Parameters.Add("p_joined_duty_on", NpgsqlDbType.Date).Value = joiningReport.JoinedDutyOn.Date;

            command.Parameters.Add("p_joining_type", NpgsqlDbType.Text).Value = joiningReport.JoiningType;

            command.Parameters.Add("p_leave_applied_till", NpgsqlDbType.Date).Value =
                (object?)joiningReport.LeaveAppliedTill?.Date ?? DBNull.Value;

            command.Parameters.Add("p_delay_reason", NpgsqlDbType.Text).Value =
                (object?)joiningReport.DelayReason ?? DBNull.Value;

            command.Parameters.Add("p_form_date", NpgsqlDbType.Date).Value = joiningReport.FormDate.Date;

            command.Parameters.Add("p_signature", NpgsqlDbType.Text).Value = joiningReport.Signature;

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
            await using var command = new NpgsqlCommand(
                "SELECT * FROM get_pending_level1_joining_reports()", connection);
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
            await using var command = new NpgsqlCommand(
                "SELECT * FROM approve_level1_joining_report(@p_report_id, @p_level1_manager_id)", connection);
            command.Parameters.Add("p_report_id", NpgsqlDbType.Integer).Value = reportId;
            command.Parameters.Add("p_level1_manager_id", NpgsqlDbType.Integer).Value = managerId.Value;

            await connection.OpenAsync();
            await using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                // Consume the function result; the database function performs the update.
            }
        }

        // LEVEL 1 REJECT
        public async Task RejectLevel1Async(int reportId, string reason)
        {
            if (string.IsNullOrWhiteSpace(reason))
                throw new ArgumentException("Rejection reason is required.");

            await using var connection = CreateConnection();
            await using var command = new NpgsqlCommand(
                "SELECT * FROM reject_level1_joining_report(@p_report_id, @p_rejection_reason)", connection);
            command.Parameters.Add("p_report_id", NpgsqlDbType.Integer).Value = reportId;
            command.Parameters.Add("p_rejection_reason", NpgsqlDbType.Text).Value = reason.Trim();

            await connection.OpenAsync();
            await using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                // Consume the function result; the database function performs the update.
            }
        }

        // LEVEL 2 APPROVE
        public async Task ApproveLevel2Async(int reportId, int managerId)
        {
            await using var connection = CreateConnection();
            await using var command = new NpgsqlCommand(
                "SELECT * FROM approve_level2_joining_report(@p_report_id, @p_level2_manager_id)", connection);
            command.Parameters.Add("p_report_id", NpgsqlDbType.Integer).Value = reportId;
            command.Parameters.Add("p_level2_manager_id", NpgsqlDbType.Integer).Value = managerId;

            await connection.OpenAsync();
            await using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                // Consume the function result; the database function performs the update.
            }
        }

        // LEVEL 2 REJECT
        public async Task RejectLevel2Async(
            int reportId, int managerId, string reason)
        {
            await using var connection = CreateConnection();
            await using var command = new NpgsqlCommand(
                "SELECT * FROM reject_level2_joining_report(@p_report_id, @p_level2_manager_id, @p_rejection_reason)", connection);
            command.Parameters.Add("p_report_id", NpgsqlDbType.Integer).Value = reportId;
            command.Parameters.Add("p_level2_manager_id", NpgsqlDbType.Integer).Value = managerId;
            command.Parameters.Add("p_rejection_reason", NpgsqlDbType.Text).Value = reason.Trim();

            await connection.OpenAsync();
            await using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                // Consume the function result; the database function performs the update.
            }
        }

        // HR APPROVE
        public async Task ApproveHrAsync(int reportId, int hrManagerId)
        {
            await using var connection = CreateConnection();
            await using var command = new NpgsqlCommand(
                "SELECT * FROM approve_hr_joining_report(@p_report_id, @p_hr_manager_id)", connection);
            command.Parameters.Add("p_report_id", NpgsqlDbType.Integer).Value = reportId;
            command.Parameters.Add("p_hr_manager_id", NpgsqlDbType.Integer).Value = hrManagerId;

            await connection.OpenAsync();
            await using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                // Consume the function result; the database function performs the update.
            }
        }

        // HR REJECT
        public async Task RejectHrAsync(
            int reportId, int hrManagerId, string reason)
        {
            await using var connection = CreateConnection();
            await using var command = new NpgsqlCommand(
                "SELECT * FROM reject_hr_joining_report(@p_report_id, @p_hr_manager_id, @p_rejection_reason)", connection);
            command.Parameters.Add("p_report_id", NpgsqlDbType.Integer).Value = reportId;
            command.Parameters.Add("p_hr_manager_id", NpgsqlDbType.Integer).Value = hrManagerId;
            command.Parameters.Add("p_rejection_reason", NpgsqlDbType.Text).Value = reason.Trim();

            await connection.OpenAsync();
            await using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                // Consume the function result; the database function performs the update.
            }
        }

        
private static JoiningReport MapJoiningReport(NpgsqlDataReader reader)
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
