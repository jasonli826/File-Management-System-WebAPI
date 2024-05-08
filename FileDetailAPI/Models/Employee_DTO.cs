using System.ComponentModel.DataAnnotations;

namespace FileDetailAPI.Models
{
    public class Employee_DTO
    {
        [Key]
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public int DepartmentId { get; set; }
        public string Department_DESC { get; set; }
        public string DateOfJoining { get; set; }
        public string PhotoFileName { get; set; }
    }
}
