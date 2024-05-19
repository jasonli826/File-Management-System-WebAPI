using System.ComponentModel.DataAnnotations;
using System;

namespace FileDetailAPI.Models
{
    public class Project_DTO
    {
        public int Project_ID { get; set; }
        [Required]
        public string Project_Name { get; set; }
        public string Created_by { get; set; }
        public DateTime Created_Date { get; set; }
        public string Updated_by { get; set; }
        public DateTime Updated_Date { get; set; }
        [Required]
        public bool isActive { get; set; }
    }
}
