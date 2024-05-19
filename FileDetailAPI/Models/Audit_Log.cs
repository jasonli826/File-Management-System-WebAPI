using System.ComponentModel.DataAnnotations;
using System;

namespace FileDetailAPI.Models
{
  public class Audit_Log
  {
    public int Id { get; set; }

    public string UserId { get; set; }

    public int ActionType { get; set; }
    
    public string FileName { get; set; }

    public string Project_Name { get; set; }

    public string LogMessage { get; set; }

    public string Created_by { get; set; }
 
    public DateTime Created_Date { get; set; }

    public string Updated_by { get; set; }

    public DateTime Updated_Date { get; set; }
  }
}
