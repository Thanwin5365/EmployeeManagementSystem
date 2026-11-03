using EmployeeJoiningAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeJoiningAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<JoiningReport> JoiningReports { get; set; }

        public DbSet<Manager> Managers { get; set; }

        public DbSet<JoiningReportApprovalHistory> JoiningReportApprovalHistory { get; set; }
    }
}