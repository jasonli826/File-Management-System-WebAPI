using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FileDetailAPI.Models
{
    public class RoleControl
    {
        [Key]
        public int RoleControID { get; set; }
        [ForeignKey("RoleId")]
        public int RoleId { get; set; }
        [ForeignKey("MenuId")]
        public int MenuId { get; set; }

        public string Created_by { get; set; }

        public DateTime Created_Date { get; set; }

        public string Updated_by { get; set; }

        public DateTime Updated_Date { get; set; }
    }
}
