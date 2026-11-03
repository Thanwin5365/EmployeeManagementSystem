namespace EmployeeJoiningAPI.Models
{
    public class JoiningReport
    {
        public int Id { get; set; }

        public int EmployeeId { get; set; }

        public string EmployeeName { get; set; } = string.Empty;

        public string Designation { get; set; } = string.Empty;

        public string Department { get; set; } = string.Empty;

        public DateTime JoinedDutyOn { get; set; }

        public string JoiningType { get; set; } = string.Empty;

        public DateTime? LeaveAppliedTill { get; set; }

        public string? DelayReason { get; set; }

        public DateTime FormDate { get; set; }

        public string Signature { get; set; } = string.Empty;

        public DateTime CreatedDate { get; set; }


        // =====================================================
        // CURRENT OVERALL STATUS
        // =====================================================

        public string ApprovalStatus { get; set; } = "Pending";


        // =====================================================
        // LEVEL 1 MANAGER
        // =====================================================

        public int? Level1ManagerId { get; set; }

        public DateTime? Level1ApprovedDate { get; set; }

        public string? Level1RejectionReason { get; set; }


        // =====================================================
        // LEVEL 2 MANAGER
        // =====================================================

        public int? Level2ManagerId { get; set; }

        public DateTime? Level2ApprovedDate { get; set; }

        public string? Level2RejectionReason { get; set; }


        // =====================================================
        // HR - FINAL APPROVAL
        // =====================================================

        public int? HrManagerId { get; set; }

        public DateTime? HrApprovedDate { get; set; }

        public string? HrRejectionReason { get; set; }
    }
}