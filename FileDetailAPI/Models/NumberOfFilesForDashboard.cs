namespace FileDetailAPI.Models
{
  public class NumberOfFilesForDashboard
  {

    public int totalNumberOfDownloadFiles { get; set; }

    public int totalNumberOfDownloadFilesByToday { get; set; }

    public int totalNumberOfUploadFiles { get; set; }

    public double uploadFilePercentage { get; set; }

    public int numberOfUsers { get; set; }

    public int numberOfNewUsers { get; set; }

    public int numberOfNewProjects{ get; set; }

    public int totalNumberOfProjects { get; set; }


  }
}
