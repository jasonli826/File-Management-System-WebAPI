using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FileDetailAPI.Models
{
    public class MenuItems
    {
        [Key]
        public int MenuID { get; set; }
        
        public string Menu_Description { get; set; }

        public string Created_by { get; set; }

        public DateTime Created_Date { get; set; }

        public string Updated_by { get; set; }

        public DateTime Updated_Date { get; set; }
    }
}
