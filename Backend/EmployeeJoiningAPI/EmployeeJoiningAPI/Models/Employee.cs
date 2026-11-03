namespace EmployeeJoiningAPI.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; }


        public string Name { get; set; }

        public string Department { get; set; }

        public decimal Salary { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime LastModifiedDate { get; set; }

        public DateTime? LastActive { get; set; }

        public bool IsActive { get; set; }

        public string PasswordHash { get; set; }

        public string? Designation { get; set; }

        public string? Email { get; set; }

        public string? Phone { get; set; }

        public string? Location { get; set; }

        public int ManagerId { get; set; }

        public string Username { get; set; } = "";
    }
}