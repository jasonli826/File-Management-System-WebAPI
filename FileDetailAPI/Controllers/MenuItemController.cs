using FileDetailAPI.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.Logging;

namespace FileDetailAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuItemController : ControllerBase
    {
     private readonly ILogger<MenuItemController> _logger;
     private readonly IMenuItemsRepository _menuItemsRepository;

        public MenuItemController(ILogger<MenuItemController> logger, IMenuItemsRepository menuItem)
        {
            _logger = logger;
            _menuItemsRepository = menuItem ?? throw new ArgumentNullException(nameof(menuItem));
        }

        [HttpGet]
        [Route("GetMenuItems")]
        public async Task<IActionResult> Get()
        {
          try
          {
              _logger.LogInformation("Starting to CallGetMenuItems");
              var menuList = await _menuItemsRepository.GetMenuItems();
              _logger.LogInformation("Ending to CallGetMenuItems");
              return Ok(menuList);
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
