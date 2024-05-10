using FileDetailAPI.Models;
using FileDetailAPI.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace FileDetailAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : Controller
    {
        private readonly IProjectRepository _project;

        public ProjectController(IProjectRepository project)
        {
            _project = project ?? throw new ArgumentNullException(nameof(project));
        }
        [HttpGet]
        [Route("GetProjectList")]
        public async Task<IActionResult> Get()
        {
            return Ok(await _project.GetProjectList());
        }

        [HttpGet]
        [Route("SearchProject")]
        public async Task<IActionResult> Get([FromQuery] Project_DTO project_dto)
        {
            return Ok(await _project.SearchProjectList(project_dto));
        }

        [HttpPost]
        [Route("AddProject")]
        public async Task<IActionResult> Post(Project_DTO project_dto)
        {
            try
            {
                var result = await _project.InsertProject(project_dto);
                if (result.Project_ID == 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Project Name is duplicated");
                }


                return new JsonResult("Added Project Successfully");
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }

        [HttpPut]
        [Route("UpdateProject")]
        public async Task<IActionResult> Put(Project_DTO project_dto)
        {
            try
            {
                await _project.UpdateProject(project_dto);
                return new JsonResult("Updated Project Successfully");
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }
    }
}
