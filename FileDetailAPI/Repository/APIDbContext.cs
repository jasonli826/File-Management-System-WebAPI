using Microsoft.EntityFrameworkCore;
using FileDetailAPI.Models;

namespace FileDetailAPI.Repository
{
    public class APIDbContext : DbContext
    {
        public APIDbContext(DbContextOptions<APIDbContext> options) : base(options)
        {

        }

        public DbSet<FileDetails> FileDetails { get; set; }

        public DbSet<Role> Role { get; set; }

        public DbSet<MenuItems> MenuItems { get; set; }

        public DbSet<RoleControl> RoleControl { get; set; }


        public DbSet<User_tbl> User_tbl { get; set; }
        public DbSet<UserRole> UserRole { get; set; }

        public DbSet<Project> Project { get; set; }

        public DbSet<Audit_Log> Audit_Log { get; set; }

    }
}
