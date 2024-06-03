using FileDetailAPI.Models;
using FileDetailAPI.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.Logging;

namespace FileDetailAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _user;
        private readonly ILogger<UserController> _logger;
        public UserController(ILogger<UserController> logger, IUserRepository user)
        {
            _logger = logger;
            _user = user ?? throw new ArgumentNullException(nameof(user));
        }

        [HttpGet]
        [Route("GetUserList")]
        public async Task<IActionResult> Get()
        {
          try
          {
            _logger.LogInformation("Starting to GetUserList");
            var userList = await _user.GetUsers();
            _logger.LogInformation("Ending to GetUserList");
            return Ok(userList);
          }
          catch (Exception ex)
          {
            _logger.LogError($"Error occurred: {ex.Message}");
            _logger.LogError($"Stack Trace: {ex.StackTrace}");
            throw;
          }
        }
        [HttpPost]
        [Route("SearchUser")]
        public async Task<IActionResult> SearchUserList([FromBody] User_DTO user_dto)
        {
          try
          {
            _logger.LogInformation("Starting to SearchUser");
            var user = await _user.SearchUsers(user_dto);
            _logger.LogInformation("Ending to SearchUser");
            return Ok(user);
          }catch (Exception ex)
          {
            _logger.LogError($"Error occurred: {ex.Message}");
            _logger.LogError($"Stack Trace: {ex.StackTrace}");
            throw;
          }
        }
        [HttpGet]
        [Route("GetUserByID/{Id}")]
        public async Task<IActionResult> GetUserByID(string userId)
        {
            try
            {
              _logger.LogInformation("Starting to GetUserByID and userId :"+userId);
              var singleUser = await _user.GetUserByID(userId);
              _logger.LogInformation("Ending to GetUserByID and userId :" + userId);
              return Ok(singleUser);
            }catch (Exception ex)
            {
                  _logger.LogError($"Error occurred: {ex.Message}");
                  _logger.LogError($"Stack Trace: {ex.StackTrace}");
                  throw;
            }
}

        [HttpPost]
        [Route("AddUser")]
        public async Task<IActionResult> Post(User_DTO user_dto)
        {
          try {
                  _logger.LogInformation("Starting to AddUser");
                  var result = await _user.InsertUser(user_dto);
                  _logger.LogInformation("Ending to AddUser");
                  if (result.UserId=="0")
                  {
                      return new JsonResult("User Id is Duplicate");
                  }
                  return new JsonResult("Add New User Successfully");
               }catch (Exception ex)
                {
                  _logger.LogError($"Error occurred: {ex.Message}");
                  _logger.LogError($"Stack Trace: {ex.StackTrace}");
                  throw;
                }
       }

        [HttpPut]
        [Route("UpdateUser")]
        public async Task<IActionResult> Put(User_DTO user_dto)
        {
            try
            {
                _logger.LogInformation("Starting to UpdateUser");
                var result = await _user.UpdateUser(user_dto);
                _logger.LogInformation("Ending to UpdateUser");
                if (result != null)
                    return new JsonResult("Updated User Successfully");
                else
                    return new JsonResult("Updated User Failed");
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
