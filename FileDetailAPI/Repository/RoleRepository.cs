using FileDetailAPI.Models;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;

namespace FileDetailAPI.Repository
{
    public interface IRoleRepository
    {
        Task<IEnumerable<Role>> GetRoles();
        Task<Role> GetRoleByID(int ID);
        Task<Role> InsertRole(Role role);
        Task<Role> UpdateRole(Role role);
        //bool DeleteDepartment(int ID);
    }
    public class RoleRepository : IRoleRepository
    {
        private readonly APIDbContext _appDBContext;

        public RoleRepository(APIDbContext context)
        {
            _appDBContext = context ?? throw new ArgumentNullException(nameof(context));
        }


        public async Task<Role> GetRoleByID(int ID)
        {
            return await _appDBContext.Role.FindAsync(ID);
        }

        public async Task<IEnumerable<Role>> GetRoles()
        {
            return await _appDBContext.Role.ToListAsync();
        }

        public async Task<Role> InsertRole(Role role)
        {
            var exsitingRole = _appDBContext.Role.Where(a => a.Role_Name == role.Role_Name);
            if (exsitingRole.Count() > 0)
            {
                role.RoleID = 500;
            }
            else
            {
                role.Created_by = "Administrator";
                role.Created_Date = DateTime.Now;
                role.Updated_by = "Administrator";
                role.Updated_Date = DateTime.Now;
                _appDBContext.Role.Add(role);
                await _appDBContext.SaveChangesAsync();
            }
            return role;

        }

        public async Task<Role> UpdateRole(Role role)
        {
            _appDBContext.Entry(role).State = EntityState.Modified;
            await _appDBContext.SaveChangesAsync();
            return role; 
        }
    }
}
