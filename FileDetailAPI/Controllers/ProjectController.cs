using FileDetailAPI.Models;
using FileDetailAPI.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace FileDetailAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : Controller
    {
      private readonly ILogger<ProjectController> _logger;
      private readonly IProjectRepository _project;

        public ProjectController(ILogger<ProjectController> logger, IProjectRepository project)
        {
            _logger = logger; 
            _project = project ?? throw new ArgumentNullException(nameof(project));
        }
        [HttpGet]
        [Route("GetProjectList")]
        public async Task<IActionResult> Get()
        {
            try
            {
              _logger.LogInformation("Starting to GetProjectList");
              var projectList = await _project.GetProjectList();
              _logger.LogInformation("Ending to GetProjectList");
              return Ok(projectList);
            }
            catch (Exception ex)
            {
              _logger.LogError($"Error occurred: {ex.Message}");
              _logger.LogError($"Stack Trace: {ex.StackTrace}");
              throw;
            }
    }

        [HttpGet]
        [Route("SearchProject")]
        public async Task<IActionResult> Get([FromQuery] Project_DTO project_dto)
        {
          try
          {
             _logger.LogInformation("Starting to GetProjectList");
              var projectList = await _project.SearchProjectList(project_dto);
             _logger.LogInformation("Ending to GetProjectList");
            return Ok(projectList);
          }
          catch (Exception ex)
          {
            _logger.LogError($"Error occurred: {ex.Message}");
            _logger.LogError($"Stack Trace: {ex.StackTrace}");
            throw;
          }
    }

        [HttpPost]
        [Route("AddProject")]
        public async Task<IActionResult> Post(Project_DTO project_dto)
        {
            try
            {

                if (string.IsNullOrEmpty(project_dto.Project_Name))
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "Project Name is duplicated");
                }
              _logger.LogInformation("Starting to AddProject");
              var result = await _project.InsertProject(project_dto);
              _logger.LogInformation("Ending to AddProject");
              if (result.Project_ID == 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Project Name is duplicated");
                }


                return new JsonResult("Added Project Successfully");
            }
            catch (Exception ex)
            {
              _logger.LogError($"Error occurred: {ex.Message}");
              _logger.LogError($"Stack Trace: {ex.StackTrace}");
              throw;
              //return new JsonResult(ex.Message);
            }
        }

        [HttpPut]
        [Route("UpdateProject")]
        public async Task<IActionResult> Put(Project_DTO project_dto)
        {
            try
            {
                _logger.LogInformation("Starting to UpdateProject");
                await _project.UpdateProject(project_dto);
                _logger.LogInformation("Ending to UpdateProject");
                return new JsonResult("The project has been updated successfully");
            }
            catch (Exception ex)
            {
              _logger.LogError($"Error occurred: {ex.Message}");
              _logger.LogError($"Stack Trace: {ex.StackTrace}");
              throw;
             // return new JsonResult(ex.Message);
            }
        }
    }
}
