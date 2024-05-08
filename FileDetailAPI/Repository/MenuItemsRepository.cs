using FileDetailAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FileDetailAPI.Repository
{
    public interface IMenuItemsRepository
    {
        Task<IEnumerable<MenuItems>> GetMenuItems();
    }
    public class MenuItemsRepository : IMenuItemsRepository
    {
        private readonly APIDbContext _appDBContext;

        public MenuItemsRepository(APIDbContext context)
        {
            _appDBContext = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<MenuItems>> GetMenuItems()
        {
            return await _appDBContext.MenuItems.ToListAsync();
        }
    }
}
