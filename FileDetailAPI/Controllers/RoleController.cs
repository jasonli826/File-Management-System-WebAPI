using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using FileDetailAPI.Models;
using FileDetailAPI.Repository;
using Microsoft.Extensions.Logging;
using Serilog.Core;

namespace FileDetailAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleRepository _role;
        private readonly ILogger<RoleController> _logger;
      public RoleController(ILogger<RoleController> logger,IRoleRepository role)
        {
          _logger = logger;
          _role = role ?? throw new ArgumentNullException(nameof(role));
        }

        [HttpGet]
        [Route("GetRoleList")]
        public async Task<IActionResult> Get()
        {
            try
            {
                  _logger.LogInformation("Starting to GetRoleList");
                   var roleList = (await _role.GetRoles());
                 _logger.LogInformation("Ending to GetRoleList");
                  return Ok(roleList);
            }
            catch (Exception ex)
            {
              _logger.LogError($"Error occurred: {ex.Message}");
              _logger.LogError($"Stack Trace: {ex.StackTrace}");
              throw;
            }
       }

        [HttpGet]
        [Route("GetRoleByID/{Id}")]
        public async Task<IActionResult> GetRoleByID(int Id)
        {
            try
            {
              _logger.LogInformation("Starting to GetRoleByID and Id :" + Id.ToString());
              var role = await _role.GetRoleByID(Id);
              _logger.LogInformation("Ending to GetRoleByID and Id :" + Id.ToString());
              return Ok(role);
            }
            catch (Exception ex)
            {
              _logger.LogError($"Error occurred: {ex.Message}");
              _logger.LogError($"Stack Trace: {ex.StackTrace}");
              throw;
            }

      //return Ok(await _role.GetRoleByID(Id));
        }

        [HttpPost]
        [Route("AddRole")]
        public async Task<IActionResult> Post(Role role)
        {
            try
            {
              _logger.LogInformation("Starting to CALL AddRoleControl");
              var result = await _role.InsertRole(role);
              if (result.RoleID == 0)
              {
                return StatusCode(StatusCodes.Status500InternalServerError, "Something Went Wrong");
              }
              else if (result.RoleID == 500)
              {
                return StatusCode(StatusCodes.Status500InternalServerError, "Role Name is duplicated");
              }
              _logger.LogInformation("Ending to CALL AddRoleControl");
              return new JsonResult("Add Role Successfully");
            }
            catch (Exception ex)
            {
              _logger.LogError($"Error occurred: {ex.Message}");
              _logger.LogError($"Stack Trace: {ex.StackTrace}");
              throw;
            }
      //var result = await _role.InsertRole(role);
      //if (result.RoleID == 0)
      //{
      //    return StatusCode(StatusCodes.Status500InternalServerError, "Something Went Wrong");
      //}
      //else if (result.RoleID == 500)
      //{
      //    return StatusCode(StatusCodes.Status500InternalServerError, "Role Name is duplicated");
      //}

      //return new JsonResult("Added Role Successfully");
    }

        [HttpPut]
        [Route("UpdateRole")]
        public async Task<IActionResult> Put(Role role)
        {
          try
          {
            _logger.LogInformation("Starting to CALL UpdateRole");
            var result = await _role.UpdateRole(role);
            _logger.LogInformation("Ending to CALL AddRoleControl");
            return new JsonResult("This role has been updated successfully");
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
