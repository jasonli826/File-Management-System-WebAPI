using FileDetailAPI.Models;
using FileDetailAPI.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Data;
using System.Threading.Tasks;

namespace FileDetailAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleControlController : ControllerBase
    {
        private readonly IRoleControlRepository _roleControl;

        public RoleControlController(IRoleControlRepository roleControl)
        {
            _roleControl = roleControl ?? throw new ArgumentNullException(nameof(roleControl));
          
        }

        [HttpGet]
        [Route("GetRoleControlByID/{Id}")]
        public async Task<IActionResult> Get(int Id)
        {
            return Ok(await _roleControl.GetRoleControlById(Id));
        }
        [HttpPost]
        [Route("AddRoleControl")]
        public async Task<IActionResult> Post(RoleControl_DTO role_dto)
        {
            var result = await _roleControl.InsertRoleControl(role_dto);

            return new JsonResult("Update Access Control Successfully");
        }
    }
}
