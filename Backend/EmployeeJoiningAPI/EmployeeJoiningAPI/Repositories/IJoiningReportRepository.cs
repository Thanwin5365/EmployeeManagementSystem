
using EmployeeJoiningAPI.Models;

namespace EmployeeJoiningAPI.Repositories
{
    public interface IJoiningReportRepository
    {
        // Retrieve reports
        Task<List<JoiningReport>> GetAllReportsAsync();

        Task<JoiningReport?> GetReportByIdAsync(int id);

        Task<List<JoiningReport>> GetReportsByEmployeeAsync(
            int employeeId);

        // Create report
        Task<JoiningReport> CreateReportAsync(
            JoiningReport joiningReport);

        // Pending reports
        Task<List<JoiningReport>> GetPendingLevel1ReportsAsync();

        Task<List<JoiningReport>> GetPendingLevel2ReportsAsync(
            int level2ManagerId);

        Task<List<JoiningReport>> GetPendingHrReportsAsync();

        // Level 1 approval
        Task ApproveLevel1Async(
            int reportId,
            int? managerId);

        Task RejectLevel1Async(
            int reportId,
            string reason);

        // Level 2 approval
        Task ApproveLevel2Async(
            int reportId,
            int managerId);

        Task RejectLevel2Async(
            int reportId,
            int managerId,
            string reason);

        // HR approval
        Task ApproveHrAsync(
            int reportId,
            int hrManagerId);

        Task RejectHrAsync(
            int reportId,
            int hrManagerId,
            string reason);

        // Approval history
        Task<List<JoiningReportApprovalHistory>>
            GetApprovalHistoryAsync(int id);
    }
}
