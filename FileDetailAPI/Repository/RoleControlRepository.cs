using FileDetailAPI.Models;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace FileDetailAPI.Repository
{
    public interface IRoleControlRepository
    {
        Task<IEnumerable<RoleControl>> GetRoleControlById(int Id);

        Task<RoleControl_DTO> InsertRoleControl(RoleControl_DTO roleControl_dto);
    }
    public class RoleControlRepository : IRoleControlRepository
    {
        private readonly APIDbContext _appDBContext;

        public RoleControlRepository(APIDbContext context)
        {
            _appDBContext = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<RoleControl>> GetRoleControlById(int Id)
        {
            return await _appDBContext.RoleControl.Where(x => x.RoleId == Id).ToListAsync();
        }

        public async Task<RoleControl_DTO> InsertRoleControl(RoleControl_DTO roleControl_dto)
        {
            var menuList = _appDBContext.RoleControl.Where(x => x.RoleId == roleControl_dto.RoleID);

            if (menuList.Count() > 0)
            {
                _appDBContext.RoleControl.RemoveRange(menuList);
                _appDBContext.SaveChanges();
            }
            foreach (var menid in roleControl_dto.MenuIds)
            {
                RoleControl roleControl = new RoleControl();
                roleControl.RoleId = roleControl_dto.RoleID;
                roleControl.MenuId = menid;
                roleControl.Created_by = roleControl_dto.Created_by;
                roleControl.Created_Date = DateTime.Now;
                roleControl.Updated_by = roleControl_dto.Updated_by;
                roleControl.Updated_Date = DateTime.Now;
                await _appDBContext.RoleControl.AddAsync(roleControl);
                await _appDBContext.SaveChangesAsync();
            }

            return roleControl_dto;
        }
    }
}
