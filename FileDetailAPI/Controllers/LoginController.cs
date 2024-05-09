using FileDetailAPI.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;

namespace FileDetailAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILoginRepository _login;

        public LoginController(ILoginRepository login)
        {
            _login = login ?? throw new ArgumentNullException(nameof(login));
        }

        [HttpGet]
        [Route("Login/{userId}/{password}")]
        public async Task<IActionResult> Get(string userId,string password)
        {
            try
            {
                var userInfo = await _login.Login(userId, password);

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
                return new JsonResult(ex.Message);
            }
        }
    }
}
