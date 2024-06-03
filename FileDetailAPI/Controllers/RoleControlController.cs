using FileDetailAPI.Models;
using FileDetailAPI.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<RoleControlController> _logger;
        public RoleControlController(ILogger<RoleControlController> logger, IRoleControlRepository roleControl)
        {
            _logger = logger; 
            _roleControl = roleControl ?? throw new ArgumentNullException(nameof(roleControl));
          
        }

        [HttpGet]
        [Route("GetRoleControlByID/{Id}")]
        public async Task<IActionResult> Get(int Id)
        {
            try
            {
              _logger.LogInformation("Starting to GetRoleControlByID and Id :"+Id.ToString());
              var role = await _roleControl.GetRoleControlById(Id);
              _logger.LogInformation("Ending to GetRoleControlByID and Id :" + Id.ToString());
              return Ok(role);
            }
            catch (Exception ex)
            {
              _logger.LogError($"Error occurred: {ex.Message}");
              _logger.LogError($"Stack Trace: {ex.StackTrace}");
              throw;
            }
        }
        [HttpPost]
        [Route("AddRoleControl")]
        public async Task<IActionResult> Post(RoleControl_DTO role_dto)
        {
            try
            {
               _logger.LogInformation("Starting to AddRoleControl");
               var result = await _roleControl.InsertRoleControl(role_dto);
                _logger.LogInformation("Ending to AddRoleControl");
               return new JsonResult("Add Access Control Successfully");
            }
            catch (Exception ex)
            {
              _logger.LogError($"Error occurred: {ex.Message}");
              _logger.LogError($"Stack Trace: {ex.StackTrace}");
              throw;
            }
       }
    }
}
