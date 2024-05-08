using System;
using System.ComponentModel.DataAnnotations;

namespace FileDetailAPI.Models

{
    public class User_tbl
    {
        [Key]
        public string UserId { get; set; }

        public string UserName { get; set; } 

        public string Password { get; set; }

        public string Created_by { get; set; }

        public DateTime Created_Date { get; set; }

        public string Updated_by { get; set; }

        public DateTime? Updated_Date { get; set; }

        public DateTime? Pwd_Change_Date { get; set; }

        public DateTime? Last_Login_Date { get; set; }

        public DateTime Logout_Date { get; set; }

        public int Loggedin { get; set; }

        public int Invalidlogin { get; set; }
        public int Acc_Lock { get; set; }

        public string Status { get; set; }

        public string Remarks { get; set; }
         
        public DateTime? Effective_from { get; set; }

        public DateTime? Effective_to { get; set; }
    }
}
