using System.ComponentModel.DataAnnotations;

namespace FileDetailAPI.Models
{
  public class ChangePassword
  {
    [Required]
    public string userId { get; set; }
    [Required]
    public string newPassword { get; set; }
  }
}
