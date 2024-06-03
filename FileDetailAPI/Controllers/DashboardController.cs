using FileDetailAPI.Models;
using FileDetailAPI.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace FileDetailAPI.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class DashboardController : Controller
  {
    private readonly IDashboardRepository _dashboard;
    private readonly ILogger<DashboardController> _logger;
    public DashboardController(ILogger<DashboardController> logger, IDashboardRepository dashboard)
    {
          _logger = logger;
          _dashboard = dashboard;
    }

    [HttpGet]
    [Route("DashboardTopData")]
    public  IActionResult Get()
    {
      try
      {
        _logger.LogInformation("Starting to Call DashboardTopData Data.");
        var dashboardTopData = _dashboard.GetNumberOfFilesForDashboard();
        _logger.LogInformation("Ending to Call DashboardTopData Data.");
        return Ok(dashboardTopData);
      }
      catch (Exception ex)
      {
        _logger.LogError($"Error occurred: {ex.Message}");
        _logger.LogError($"Stack Trace: {ex.StackTrace}");
        throw;
        //return new JsonResult("Exception:" + ex.Message);
      }
    }
    [HttpGet]
    [Route("GetAuditLog")]
    public IActionResult GetAuditLog()
    {
      try
      {
        _logger.LogInformation("Starting to Call Get AuditLog Data.");
        var auditLog = _dashboard.GetAuditLog();
        _logger.LogInformation("Ending to Call Get AuditLog Data.");
        return Ok(auditLog);
      }
      catch (Exception ex)
      {
        _logger.LogError($"Error occurred: {ex.Message}");
        _logger.LogError($"Stack Trace: {ex.StackTrace}");
        throw;
      }
    }
    [HttpGet]
    [Route("GetPopularFiles")]
    public IActionResult GetPopularFiles()
    {
      try
      {
        _logger.LogInformation("Starting to Call Get AuditLog Data.");
        var popularFiles = _dashboard.GetPopularDownloadFiles();
         return Ok(popularFiles);
      }catch (Exception ex)
      {
        _logger.LogError($"Error occurred: {ex.Message}");
        _logger.LogError($"Stack Trace: {ex.StackTrace}");
        throw;

      }
    }
  }
}
