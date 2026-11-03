namespace EmployeeAPI.Models;

public class EmployeePunch
{
    public int PunchId { get; set; }

    public int EmployeeId { get; set; }

    public string PunchType { get; set; } = "";

    public DateTime PunchTime { get; set; }
}