using System.ComponentModel.DataAnnotations;

namespace FileDetailAPI.Models
{
    public class LoginUser
    {
        [Required]
        public string userName { get; set; }
        [Required]
        public string password { get; set; }
    }
}
