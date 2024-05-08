using FileDetailAPI.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;

namespace FileDetailAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuItemController : ControllerBase
    {
        private readonly IMenuItemsRepository _menuItemsRepository;

        public MenuItemController(IMenuItemsRepository menuItem)
        {
            _menuItemsRepository = menuItem ?? throw new ArgumentNullException(nameof(menuItem));
        }

        [HttpGet]
        [Route("GetMenuItems")]
        public async Task<IActionResult> Get()
        {
            return Ok(await _menuItemsRepository.GetMenuItems());
        }
    }
}
