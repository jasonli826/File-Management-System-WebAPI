using System.Collections.Generic;

namespace FileDetailAPI.Models
{
    public class RoleControl_DTO
    {
        public int RoleID { get; set; }
        public List<int> MenuIds { get; set; }
        public string Created_by { get; set; }
        public string Updated_by { get; set; }
    }
}
