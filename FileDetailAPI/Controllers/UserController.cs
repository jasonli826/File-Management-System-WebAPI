using FileDetailAPI.Models;
using FileDetailAPI.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;

namespace FileDetailAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _user;

        public UserController(IUserRepository user)
        {
            _user = user ?? throw new ArgumentNullException(nameof(user));
        }

        [HttpGet]
        [Route("GetUserList")]
        public async Task<IActionResult> Get()
        {
            return Ok(await _user.GetUsers());
        }
        [HttpPost]
        [Route("SearchUser")]
        public async Task<IActionResult> SearchUserList([FromBody] User_DTO user_dto)
        {
            var user =await  _user.SearchUsers(user_dto);

            return Ok(user);
        }
        [HttpGet]
        [Route("GetUserByID/{Id}")]
        public async Task<IActionResult> GetUserByID(string userId)
        {
            return Ok(await _user.GetUserByID(userId));
        }

        [HttpPost]
        [Route("AddUser")]
        public async Task<IActionResult> Post(User_DTO user_dto)
        {
            var result = await _user.InsertUser(user_dto);
            if (result.UserId=="0")
            {
                return new JsonResult("User Id is Duplicate");
            }
            return new JsonResult("Add New User Successfully");
        }

        [HttpPut]
        [Route("UpdateUser")]
        public async Task<IActionResult> Put(User_DTO user_dto)
        {
            try
            {
                var result = await _user.UpdateUser(user_dto);
                if (result != null)
                    return new JsonResult("Updated User Successfully");
                else
                    return new JsonResult("Updated User Failed");
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }
    }
}
