using System;
using System.ComponentModel.DataAnnotations;

namespace FileDetailAPI.Models
{
    public class Role
    {
        [Key]
        public int RoleID { get; set; }

        public string Role_Name { get; set; }

        public string Description { get; set; }

        public string Created_by { get; set; }

        public DateTime Created_Date { get; set; }

        public string Updated_by { get; set; }

        public DateTime Updated_Date { get; set; }

        public string Status { get; set; }


    }
}
