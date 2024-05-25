using Microsoft.AspNetCore.SignalR;

namespace FileDetailAPI.Models
{
  public class PopularDownloadFiles
  {
    public string name { get; set; }
    public string description { get; set; }

    public int NumberOfDownloadTimes { get; set; }
    public double percentage { get; set; }

    public string color { get; set; }

    public string cssClass { get; set; }



  }
}
