namespace EmployeeJoiningAPI.Models
{
    public class Manager
    {
        public int ManagerId { get; set; }

        public string Name { get; set; } = "";

        public string Department { get; set; } = "";

        public int ManagerLevel { get; set; }

        public string PasswordHash { get; set; } = "";

        public bool IsActive { get; set; }

        public DateTime CreatedDate { get; set; }

        public string ManagerKey { get; set; } = "";
        
        public string Username { get; set; } = "";
        
    }
}