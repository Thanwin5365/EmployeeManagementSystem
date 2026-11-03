
namespace EmployeeJoiningAPI.Models
{
    public class AttendanceResponse
    {
        public int AttendanceId { get; set; }

        public int EmployeeId { get; set; }

        public DateTime AttendanceDate { get; set; }

        public DateTime? FinalCheckIn { get; set; }

        public DateTime? FinalCheckOut { get; set; }

        public decimal TotalWorkingHours { get; set; }

        public string Status { get; set; } = "Absent";
    }
}
