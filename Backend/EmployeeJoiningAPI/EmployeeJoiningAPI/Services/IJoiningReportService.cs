using EmployeeJoiningAPI.Models;

namespace EmployeeJoiningAPI.Services
{
    public interface IJoiningReportService
    {
        // =====================================================
        // GET ALL JOINING REPORTS
        // =====================================================

        Task<List<JoiningReport>> GetAllReportsAsync();


        // =====================================================
        // GET JOINING REPORT BY ID
        // =====================================================

        Task<JoiningReport?> GetReportByIdAsync(int id);


        // =====================================================
        // GET JOINING REPORTS BY EMPLOYEE ID
        // =====================================================

        Task<List<JoiningReport>> GetReportsByEmployeeAsync(
            int employeeId);


        // =====================================================
        // CREATE JOINING REPORT
        // =====================================================

        Task<JoiningReport> CreateReportAsync(
            JoiningReport joiningReport);


        // =====================================================
        // LEVEL 1 MANAGER
        // =====================================================

        Task<List<JoiningReport>> GetPendingLevel1ReportsAsync();

        Task<object> ApproveLevel1Async(
            int reportId,
            int? managerId);

        Task<object> RejectLevel1Async(
            int reportId,
            string reason);


        // =====================================================
        // LEVEL 2 MANAGER
        // =====================================================

        Task<List<JoiningReport>> GetPendingLevel2ReportsAsync(
            int level2ManagerId);

        Task<object> ApproveLevel2Async(
            int reportId,
            int managerId);

        Task<object> RejectLevel2Async(
            int reportId,
            int managerId,
            string reason);


        // =====================================================
        // HR - FINAL APPROVAL
        // =====================================================

        Task<List<JoiningReport>> GetPendingHrReportsAsync();

        Task<object> ApproveHrAsync(
            int reportId,
            int hrManagerId);

        Task<object> RejectHrAsync(
            int reportId,
            int hrManagerId,
            string reason);

        // =====================================================
        // APPROVAL HISTORY
        // =====================================================

        Task<List<JoiningReportApprovalHistory>>
            GetApprovalHistoryAsync(int id);
    }

}