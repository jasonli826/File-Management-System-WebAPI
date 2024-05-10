using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using FileDetailAPI.Models;
using FileDetailAPI.Repository;

namespace FileDetailAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleRepository _role;

        public RoleController(IRoleRepository role)
        {
            _role = role ?? throw new ArgumentNullException(nameof(role));
        }

        [HttpGet]
        [Route("GetRoleList")]
        public async Task<IActionResult> Get()
        {
            return Ok(await _role.GetRoles());
        }

        [HttpGet]
        [Route("GetRoleByID/{Id}")]
        public async Task<IActionResult> GetRoleByID(int Id)
        {
            return Ok(await _role.GetRoleByID(Id));
        }

        [HttpPost]
        [Route("AddRole")]
        public async Task<IActionResult> Post(Role role)
        {
            var result = await _role.InsertRole(role);
            if (result.RoleID == 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Something Went Wrong");
            }
            else if (result.RoleID == 500)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Role Name is duplicated");
            }
            
            return new JsonResult("Added Role Successfully");
        }

        [HttpPut]
        [Route("UpdateRole")]
        public async Task<IActionResult> Put(Role role)
        {
            await _role.UpdateRole(role);
            return new JsonResult("Updated Role Successfully");
        }

    }
}
