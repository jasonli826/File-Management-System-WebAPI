using FileDetailAPI.Models;
using FileDetailAPI.Repository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace FileDetailAPI.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class DashboardController : Controller
  {
    private readonly IDashboardRepository _dashboard;

    public DashboardController(IDashboardRepository dashboard)
    {
      _dashboard = dashboard;
    }

    [HttpGet]
    [Route("DashboardTopData")]
    public  IActionResult Get()
    {
      try
      {
        var dashboardTopData = _dashboard.GetNumberOfFilesForDashboard();

        return Ok(dashboardTopData);
      }
      catch (Exception ex)
      {
        return new JsonResult("Exception:" + ex.Message);
      }
    }
    [HttpGet]
    [Route("GetAuditLog")]
    public  IActionResult GetAuditLog()
    {
      return Ok( _dashboard.GetAuditLog());
    }
    [HttpGet]
    [Route("GetPopularFiles")]
    public IActionResult GetPopularFiles()
    {
      return Ok( _dashboard.GetPopularDownloadFiles());
    }
  }
}
