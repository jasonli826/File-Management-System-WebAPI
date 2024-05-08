using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FileDetailAPI.Models
{
    [Table("Employee_Angular")]
    public class EmployeeAngular
    {
        [Key]
        public int EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public string Department { get; set; }
        public string EmailId { get; set; }
        public string DateOfJoining { get; set; }

        public string PhotoFileName { get; set; }


        public override string ToString()
        {
            return EmployeeName;
        }
    }
}
