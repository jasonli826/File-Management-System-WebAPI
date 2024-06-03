using FileDetailAPI.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.Logging;
using Serilog.Core;

namespace FileDetailAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILoginRepository _login;
        private readonly ILogger<LoginController> _logger;
        public LoginController(ILogger<LoginController> logger,ILoginRepository login)
        {
            _logger = logger;
            _login = login ?? throw new ArgumentNullException(nameof(login));
        }

        [HttpGet]
        [Route("Login/{userId}/{password}")]
        public async Task<IActionResult> Get(string userId,string password)
        {
            byte[] data = null;
            string decodedString = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(password))
                {
                  data = Convert.FromBase64String(password);
                  decodedString = System.Text.Encoding.UTF8.GetString(data);
                }
                _logger.LogInformation("Starting to Call Login Method UserId:"+userId);
                var userInfo = await _login.Login(userId, decodedString);
                _logger.LogInformation("Ending to Call Login Method UserId:" + userId);
                if (userInfo.userId == "0")
                {
                    return new JsonResult("User Name or Password is invalid");
                }
                if (userInfo.userId == "Inactive User")
                {
                    return new JsonResult("This User Is Inactive");
                }

                return Ok(userInfo);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred: {ex.Message}");
                _logger.LogError($"Stack Trace: {ex.StackTrace}");
                throw;
                 // return new JsonResult("Exception:"+ex.Message);
            }
        }
    }
}
