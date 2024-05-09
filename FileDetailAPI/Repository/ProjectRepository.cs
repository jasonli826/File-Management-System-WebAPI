using FileDetailAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace FileDetailAPI.Repository
{
    public interface IProjectRepository
    {
        Task<IEnumerable<Project>> GetProjectList();
        Task<Project> InsertProject(Project project);
        Task<Project> UpdateProject(Project project);

    }
    public class ProjectRepository : IProjectRepository
    {
        private readonly APIDbContext _appDBContext;

        public ProjectRepository(APIDbContext context)
        {
            _appDBContext = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<Project>> GetProjectList()
        {
            return await _appDBContext.Project.ToListAsync();
        }


        public async Task<Project> InsertProject(Project project)
        {
            var exsitingProject = _appDBContext.Project.Where(a => a.Project_Name == project.Project_Name);
            if (exsitingProject.Count() > 0)
            {
                project.Project_ID = 0;
            }
            else
            {

                project.Created_Date = DateTime.Now;
                project.Updated_by = project.Created_by;
                project.Updated_Date = DateTime.Now;
                _appDBContext.Project.Add(project);
                await _appDBContext.SaveChangesAsync();
            }
            return project;
        }

        public async Task<Project> UpdateProject(Project project)
        {
            _appDBContext.Entry(project).State = EntityState.Modified;
            await _appDBContext.SaveChangesAsync();
            return project;
        }
    }
}
