using FileDetailAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Data.Entity;

namespace FileDetailAPI.Repository
{
  public interface IDashboardRepository
  {
    NumberOfFilesForDashboard GetNumberOfFilesForDashboard();
    IEnumerable<Audit_Log> GetAuditLog();
    IEnumerable<PopularDownloadFiles> GetPopularDownloadFiles();
  }
  public class DashboardRepository : IDashboardRepository
  {
    private readonly APIDbContext _appDBContext;
    public DashboardRepository(APIDbContext context)
    {
      _appDBContext = context ?? throw new ArgumentNullException(nameof(context));
    }
    public NumberOfFilesForDashboard GetNumberOfFilesForDashboard()
    {
        double total = 0;
        double lastWeek = 0;

        NumberOfFilesForDashboard numberOfFiles = new NumberOfFilesForDashboard();
      try
      {
        numberOfFiles.totalNumberOfDownloadFiles =  _appDBContext.Audit_Log.Where(x => x.ActionType == 1).Count();
        numberOfFiles.totalNumberOfDownloadFilesByToday =  _appDBContext.Audit_Log.Where(x => x.ActionType == 1 && x.Created_Date >= DateTime.Today && x.Created_Date < DateTime.Today.AddDays(1)).Count();
        numberOfFiles.totalNumberOfUploadFiles =  _appDBContext.Audit_Log.Where(x => x.ActionType == 2).Count();
        total = numberOfFiles.totalNumberOfUploadFiles;
        lastWeek =  _appDBContext.Audit_Log.Where(x => x.ActionType == 2 && x.Created_Date >= DateTime.Today.AddDays(-7) && x.Created_Date <= DateTime.Now).Count();
        numberOfFiles.uploadFilePercentage = Math.Round(lastWeek / total*100,2);
        numberOfFiles.numberOfUsers =  _appDBContext.User_tbl.Count();
        numberOfFiles.numberOfNewUsers =  _appDBContext.User_tbl.Where(x => x.Created_Date >= DateTime.Today.AddDays(-7) && x.Created_Date <= DateTime.Now).Count();
        numberOfFiles.totalNumberOfProjects =  _appDBContext.Project.Count();
        numberOfFiles.numberOfNewProjects =  _appDBContext.Project.Where(x => x.Created_Date >= DateTime.Today.AddDays(-7) && x.Created_Date <= DateTime.Now).Count();
      }
      catch (Exception ex)
      {
        throw ex;
      }
        return numberOfFiles;
    }

    public  IEnumerable<Audit_Log> GetAuditLog()
    {
      return  _appDBContext.Audit_Log.OrderByDescending(x=>x.Created_Date).ToList();
    }

    public  IEnumerable<PopularDownloadFiles> GetPopularDownloadFiles()
    {

      var PopularDownloadFiles =  _appDBContext.Audit_Log
         .Where(a => a.ActionType == 1)
         .GroupBy(a => new { a.FileName, a.Project_Name })
         .Select(g => new PopularDownloadFiles
         {
           name = g.Key.FileName,
           description = g.Key.Project_Name,
           NumberOfDownloadTimes = g.Count()
         })
         .OrderByDescending(s => s.NumberOfDownloadTimes)
         .Take(5)
         .ToList();

      int totalDownlodFiles = 1;
      totalDownlodFiles= _appDBContext.Audit_Log.Where(x => x.ActionType == 1).Count();
      int i = 0;
      foreach (var file in PopularDownloadFiles)
      {
        file.percentage = Math.Round(((double)file.NumberOfDownloadTimes /(double) totalDownlodFiles) * 100,2);
        if (i == 0)
        {
          file.color = "bg-orange-500 h-full";
          file.cssClass = "text-orange-500 ml-3 font-medium";
        }
        else if (i == 1)
        {
          file.color = "bg-cyan-500 h-full";
          file.cssClass = "text-cyan-500 ml-3 font-medium";
        }
        else if (i == 2)
        {
          file.color = "bg-pink-500 h-full";
          file.cssClass = "text-pink-500 ml-3 font-medium";
        }
        else if (i == 3)
        {
          file.color = "bg-green-500 h-full";
          file.cssClass = "text-green-500 ml-3 font-medium";
        }
        else if (i == 4)
        {
          file.color = "bg-purple-500 h-full";
          file.cssClass = "text-purple-500 ml-3 font-medium";
        }

        i++;
      }
      return PopularDownloadFiles;

    }
  }
}
