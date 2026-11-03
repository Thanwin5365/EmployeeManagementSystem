namespace EmployeeJoiningAPI.Models
{
    public class JoiningReportApprovalHistory
    {
        public int ApprovalHistoryId { get; set; }

        public int Id { get; set; }

        public int ManagerId { get; set; }

        public string? ManagerName { get; set; }

        public string ManagerLevel { get; set; } = string.Empty;

        public int ApprovalLevel { get; set; }

        public string ApprovalStatus { get; set; } = string.Empty;

        public string? RejectionReason { get; set; }

        public string? ModifiedBy { get; set; }

        public DateTime ModifiedDate { get; set; }
    }
}