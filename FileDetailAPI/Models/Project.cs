using System;
using System.ComponentModel.DataAnnotations;

namespace FileDetailAPI.Models
{
    public class Project
    {
        [Key]
        public int Project_ID { get; set; }
        [Required]
        public string Project_Name { get; set; }
        [Required]
        public string Created_by { get; set; }
        [Required]
        public DateTime Created_Date { get; set; }

        public string Updated_by { get; set; }

        public DateTime Updated_Date { get; set; }
        [Required]
        public string Status { get; set; }
    }
}
