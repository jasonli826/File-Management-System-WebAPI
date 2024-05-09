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


        [HttpPost]
        [Route("AddProject")]
        public async Task<IActionResult> Post(Project project)
        {
            var result = await _project.InsertProject(project);
            if (result.Project_ID == 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Role Name is duplicated");
            }
 

            return new JsonResult("Added Project Successfully");
        }

        [HttpPut]
        [Route("UpdateProject")]
        public async Task<IActionResult> Put(Project project)
        {
            await _project.UpdateProject(project);
            return new JsonResult("Updated Project Successfully");
        }
    }
}
