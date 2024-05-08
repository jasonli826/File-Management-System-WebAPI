using System;
using System.Collections.Generic;
using System.Security.Policy;

namespace FileDetailAPI.Models
{
    public class User_DTO
    {

        public string UserId { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public bool isActive { get; set; }

        public string Created_by { get; set; }

        public DateTime Created_Date { get; set; }
        public DateTime? Update_Date { get; set; }
        public string Update_by { get; set; }

        public List<int> RoleIds { get; set; }

        public string? EffectiveDateFrom { get; set; }


        public string? EffectiveDateEnd { get; set; }

        public string Remark { get; set; }


    }
}
