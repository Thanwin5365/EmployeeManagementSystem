using EmployeeJoiningAPI.Models;

namespace EmployeeJoiningAPI.Services
{
    public interface IJoiningReportApprovalHistoryService
    {
        
        // GET APPROVAL HISTORY BY JOINING REPORT ID
        

        Task<List<JoiningReportApprovalHistory>> GetApprovalHistoryAsync(
            int joiningReportId);
    }
}