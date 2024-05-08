using Microsoft.EntityFrameworkCore;
using FileDetailAPI.Models;

namespace FileDetailAPI.Repository
{
    public class APIDbContext : DbContext
    {
        public APIDbContext(DbContextOptions<APIDbContext> options) : base(options)
        {

        }

        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employee { get; set; }
        public DbSet<EmployeeAngular> EmployeeAngular { get; set; }

        public DbSet<FileDetails> FileDetails { get; set; }

        public DbSet<Role> Role { get; set; }

        public DbSet<MenuItems> MenuItems { get; set; }

        public DbSet<RoleControl> RoleControl { get; set; }


        public DbSet<User_tbl> User_tbl { get; set; }
        public DbSet<UserRole> UserRole { get; set; }

    }
}
