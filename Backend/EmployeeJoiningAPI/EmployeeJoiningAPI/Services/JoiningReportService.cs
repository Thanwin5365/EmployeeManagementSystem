
using EmployeeJoiningAPI.Models;
using EmployeeJoiningAPI.Repositories;

namespace EmployeeJoiningAPI.Services
{
    public class JoiningReportService : IJoiningReportService
    {
        private readonly IJoiningReportRepository _repository;
        private readonly IEmailService _emailService;
        private readonly IEmployeeRepository _employeeRepository;

        public JoiningReportService(
    IJoiningReportRepository repository,
    IEmailService emailService,
    IEmployeeRepository employeeRepository)
        {
            _repository = repository;
            _emailService = emailService;
            _employeeRepository = employeeRepository;
        }

        // GET ALL REPORTS

        public async Task<List<JoiningReport>> GetAllReportsAsync()
        {
            return await _repository.GetAllReportsAsync();
        }

        // GET REPORT BY ID

        public async Task<JoiningReport?> GetReportByIdAsync(int id)
        {
            if (id <= 0)
                throw new Exception("Invalid joining report ID.");

            return await _repository.GetReportByIdAsync(id);
        }

        // GET REPORTS BY EMPLOYEE

        public async Task<List<JoiningReport>> GetReportsByEmployeeAsync(
            int employeeId)
        {
            if (employeeId <= 0)
                throw new Exception("Invalid employee ID.");

            return await _repository.GetReportsByEmployeeAsync(employeeId);
        }

        // CREATE JOINING REPORT

        public async Task<JoiningReport> CreateReportAsync(
            JoiningReport joiningReport)
        {
            if (joiningReport == null)
                throw new ArgumentNullException(nameof(joiningReport));

            // Initialize the approval workflow.
            joiningReport.ApprovalStatus = "Pending";

            joiningReport.Level1ManagerId = null;
            joiningReport.Level1ApprovedDate = null;
            joiningReport.Level1RejectionReason = null;

            joiningReport.Level2ManagerId = null;
            joiningReport.Level2ApprovedDate = null;
            joiningReport.Level2RejectionReason = null;

            joiningReport.HrManagerId = null;
            joiningReport.HrApprovedDate = null;
            joiningReport.HrRejectionReason = null;

            joiningReport.CreatedDate = DateTime.Now;

            return await _repository.CreateReportAsync(joiningReport);
        }

        // GET LEVEL 1 PENDING REPORTS

        public async Task<List<JoiningReport>>
            GetPendingLevel1ReportsAsync()
        {
            return await _repository.GetPendingLevel1ReportsAsync();
        }

        // GET LEVEL 2 PENDING REPORTS

        public async Task<List<JoiningReport>>
            GetPendingLevel2ReportsAsync(int level2ManagerId)
        {
            if (level2ManagerId <= 0)
                throw new Exception("Level 2 Manager ID is required.");

            return await _repository.GetPendingLevel2ReportsAsync(
                level2ManagerId);
        }

        // LEVEL 1 APPROVE

        public async Task<object> ApproveLevel1Async(
            int reportId,
            int? managerId)
        {
            if (reportId <= 0)
                throw new Exception("Invalid joining report ID.");

            if (managerId == null || managerId <= 0)
                throw new Exception("Level 1 Manager ID is required.");

            var report = await _repository.GetReportByIdAsync(reportId);

            if (report == null)
                throw new Exception("Joining report not found.");

            if (report.ApprovalStatus != "Pending")
                throw new Exception(
                    $"This joining report is already {report.ApprovalStatus}.");

            await _repository.ApproveLevel1Async(
                reportId,
                managerId);

            var updatedReport =
                await _repository.GetReportByIdAsync(reportId);

            if (updatedReport == null)
                throw new Exception(
                    "Joining report could not be retrieved after approval.");

            await SendJoiningReportEmailAsync(
    updatedReport,
    $"Joining Report #{updatedReport.Id} - Level 1 Approved",
    $"Hello,\n\n" +
    $"Your Joining Report #{updatedReport.Id} has been approved " +
    $"by the Level 1 Manager and forwarded to the Level 2 Manager.\n\n" +
    $"Current Status: {updatedReport.ApprovalStatus}\n\n" +
    "Regards,\nAlpha Data Employee Portal");

            return new
            {
                message =
                    "Joining report approved by Level 1 manager " +
                    "and sent to Level 2 manager.",
                reportId = updatedReport.Id,
                employeeId = updatedReport.EmployeeId,
                approvalStatus = updatedReport.ApprovalStatus,
                level1ManagerId = updatedReport.Level1ManagerId,
                level2ManagerId = updatedReport.Level2ManagerId,
                level1ApprovedDate = updatedReport.Level1ApprovedDate
            };
        }
        
        // LEVEL 1 REJECT

        public async Task<object> RejectLevel1Async(
            int reportId,
            string reason)
        {
            if (reportId <= 0)
                throw new Exception("Invalid joining report ID.");

            if (string.IsNullOrWhiteSpace(reason))
                throw new Exception("Rejection reason is required.");

            var report = await _repository.GetReportByIdAsync(reportId);

            if (report == null)
                throw new Exception("Joining report not found.");

            if (report.ApprovalStatus != "Pending")
                throw new Exception(
                    $"This joining report is already {report.ApprovalStatus}.");

            await _repository.RejectLevel1Async(
                reportId,
                reason.Trim());

            var updatedReport =
                await _repository.GetReportByIdAsync(reportId);

            if (updatedReport == null)
                throw new Exception(
                    "Joining report could not be retrieved after rejection.");

            await SendJoiningReportEmailAsync(
    updatedReport,
    $"Joining Report #{updatedReport.Id} - Level 1 Rejected",
    $"Hello,\n\n" +
    $"Your Joining Report #{updatedReport.Id} has been rejected " +
    $"by the Level 1 Manager.\n\n" +
    $"Reason: {updatedReport.Level1RejectionReason}\n\n" +
    $"Current Status: {updatedReport.ApprovalStatus}\n\n" +
    "Regards,\nAlpha Data Employee Portal");

            return new
            {
                message = "Joining report rejected by Level 1 manager.",
                reportId = updatedReport.Id,
                employeeId = updatedReport.EmployeeId,
                approvalStatus = updatedReport.ApprovalStatus,
                level1RejectionReason =
                    updatedReport.Level1RejectionReason
            };
        }

        // LEVEL 2 APPROVE

        public async Task<object> ApproveLevel2Async(
            int reportId,
            int managerId)
        {
            if (reportId <= 0)
                throw new Exception("Invalid joining report ID.");

            if (managerId <= 0)
                throw new Exception("Level 2 Manager ID is required.");

            var report = await _repository.GetReportByIdAsync(reportId);

            if (report == null)
                throw new Exception("Joining report not found.");

            if (report.ApprovalStatus != "Level1Approved")
                throw new Exception(
                    $"This joining report cannot be approved by Level 2. " +
                    $"Current status: {report.ApprovalStatus}");

            if (report.Level2ManagerId != managerId)
                throw new Exception(
                    "This joining report is not assigned to this Level 2 manager.");

            await _repository.ApproveLevel2Async(reportId, managerId);

            var updatedReport =
                await _repository.GetReportByIdAsync(reportId);

            if (updatedReport == null)
                throw new Exception(
                    "Joining report could not be retrieved after approval.");


            await SendJoiningReportEmailAsync(
                updatedReport,
                $"Joining Report #{updatedReport.Id} - Level 2 Approved",
                $"Hello,\n\n" +
                $"Your Joining Report #{updatedReport.Id} has been approved " +
                $"by the Level 2 Manager and forwarded to HR for final review.\n\n" +
                $"Current Status: {updatedReport.ApprovalStatus}\n\n" +
                "Regards,\nAlpha Data Employee Portal");


            return new
            {
                message =
                    "Joining report approved by Level 2 manager " +
                    "and sent to HR.",
                reportId = updatedReport.Id,
                employeeId = updatedReport.EmployeeId,
                approvalStatus = updatedReport.ApprovalStatus,
                level2ManagerId = updatedReport.Level2ManagerId,
                level2ApprovedDate = updatedReport.Level2ApprovedDate
            };
        }

        // LEVEL 2 REJECT

        public async Task<object> RejectLevel2Async(
            int reportId,
            int managerId,
            string reason)
        {
            if (reportId <= 0)
                throw new Exception("Invalid joining report ID.");

            if (managerId <= 0)
                throw new Exception("Level 2 Manager ID is required.");

            if (string.IsNullOrWhiteSpace(reason))
                throw new Exception("Rejection reason is required.");

            var report = await _repository.GetReportByIdAsync(reportId);

            if (report == null)
                throw new Exception("Joining report not found.");

            if (report.ApprovalStatus != "Level1Approved")
                throw new Exception(
                    $"This joining report cannot be rejected by Level 2. " +
                    $"Current status: {report.ApprovalStatus}");

            if (report.Level2ManagerId != managerId)
                throw new Exception(
                    "This joining report is not assigned to this Level 2 manager.");

            await _repository.RejectLevel2Async(
                reportId,
                managerId,
                reason.Trim());

            var updatedReport =
                await _repository.GetReportByIdAsync(reportId);

            if (updatedReport == null)
                throw new Exception(
                    "Joining report could not be retrieved after rejection.");


            await SendJoiningReportEmailAsync(
                updatedReport,
                $"Joining Report #{updatedReport.Id} - Level 2 Rejected",
                $"Hello,\n\n" +
                $"Your Joining Report #{updatedReport.Id} has been rejected " +
                $"by the Level 2 Manager.\n\n" +
                $"Reason: {updatedReport.Level2RejectionReason}\n\n" +
                $"Current Status: {updatedReport.ApprovalStatus}\n\n" +
                "Regards,\nAlpha Data Employee Portal");


            return new
            {
                message = "Joining report rejected by Level 2 manager.",
                reportId = updatedReport.Id,
                employeeId = updatedReport.EmployeeId,
                approvalStatus = updatedReport.ApprovalStatus,
                level2RejectionReason =
                    updatedReport.Level2RejectionReason
            };
        }

        // GET HR PENDING REPORTS

        public async Task<List<JoiningReport>>
            GetPendingHrReportsAsync()
        {
            return await _repository.GetPendingHrReportsAsync();
        }

        // HR APPROVE - FINAL

        public async Task<object> ApproveHrAsync(
            int reportId,
            int hrManagerId)
        {
            if (reportId <= 0)
                throw new Exception("Invalid joining report ID.");

            if (hrManagerId <= 0)
                throw new Exception("HR Manager ID is required.");

            var report = await _repository.GetReportByIdAsync(reportId);

            if (report == null)
                throw new Exception("Joining report not found.");

            if (report.ApprovalStatus != "Level2Approved")
                throw new Exception(
                    $"This joining report cannot be approved by HR. " +
                    $"Current status: {report.ApprovalStatus}");

            await _repository.ApproveHrAsync(reportId, hrManagerId);

            var updatedReport =
                await _repository.GetReportByIdAsync(reportId);

            if (updatedReport == null)
                throw new Exception(
                    "Joining report could not be retrieved after HR approval.");


            await SendJoiningReportEmailAsync(
                updatedReport,
                $"Joining Report #{updatedReport.Id} - HR Approved",
                $"Hello,\n\n" +
                $"Your Joining Report #{updatedReport.Id} has received final approval from HR.\n\n" +
                $"Current Status: {updatedReport.ApprovalStatus}\n\n" +
                "Regards,\nAlpha Data Employee Portal");


            return new
            {
                message = "Joining report finally approved by HR.",
                reportId = updatedReport.Id,
                employeeId = updatedReport.EmployeeId,
                approvalStatus = updatedReport.ApprovalStatus,
                hrManagerId = updatedReport.HrManagerId,
                hrApprovedDate = updatedReport.HrApprovedDate
            };
        }

        // HR REJECT - FINAL

        public async Task<object> RejectHrAsync(
            int reportId,
            int hrManagerId,
            string reason)
        {
            if (reportId <= 0)
                throw new Exception("Invalid joining report ID.");

            if (hrManagerId <= 0)
                throw new Exception("HR Manager ID is required.");

            if (string.IsNullOrWhiteSpace(reason))
                throw new Exception("Rejection reason is required.");

            var report = await _repository.GetReportByIdAsync(reportId);

            if (report == null)
                throw new Exception("Joining report not found.");

            if (report.ApprovalStatus != "Level2Approved")
                throw new Exception(
                    $"This joining report cannot be rejected by HR. " +
                    $"Current status: {report.ApprovalStatus}");

            await _repository.RejectHrAsync(
                reportId,
                hrManagerId,
                reason.Trim());

            var updatedReport =
                await _repository.GetReportByIdAsync(reportId);

            if (updatedReport == null)
                throw new Exception(
                    "Joining report could not be retrieved after HR rejection.");


            await SendJoiningReportEmailAsync(
                updatedReport,
                $"Joining Report #{updatedReport.Id} - HR Rejected",
                $"Hello,\n\n" +
                $"Your Joining Report #{updatedReport.Id} has been rejected by HR.\n\n" +
                $"Reason: {updatedReport.HrRejectionReason}\n\n" +
                $"Current Status: {updatedReport.ApprovalStatus}\n\n" +
                "Regards,\nAlpha Data Employee Portal");


            return new
            {
                message = "Joining report finally rejected by HR.",
                reportId = updatedReport.Id,
                employeeId = updatedReport.EmployeeId,
                approvalStatus = updatedReport.ApprovalStatus,
                hrManagerId = updatedReport.HrManagerId,
                hrRejectionReason = updatedReport.HrRejectionReason
            };
        }

        // GET APPROVAL HISTORY

        public async Task<List<JoiningReportApprovalHistory>>
            GetApprovalHistoryAsync(int id)
        {
            if (id <= 0)
                throw new Exception("Invalid joining report ID.");

            return await _repository.GetApprovalHistoryAsync(id);
        }

        private async Task SendJoiningReportEmailAsync(
    JoiningReport report,
    string subject,
    string message)
        {
            var employee = await _employeeRepository
                .GetEmployeeByIdAsync(report.EmployeeId);

            if (employee == null ||
                string.IsNullOrWhiteSpace(employee.Email))
            {
                throw new InvalidOperationException(
                    $"Email address not found for employee ID {report.EmployeeId}.");
            }

            await _emailService.SendEmailAsync(
                employee.Email,
                subject,
                message);
        }
    }
}
