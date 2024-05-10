using FileDetailAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Mvc;

namespace FileDetailAPI.Repository
{
    public interface IProjectRepository
    {
        Task<IEnumerable<Project>> GetProjectList();
        Task<Project> InsertProject(Project_DTO project_dto);
        Task<Project> UpdateProject(Project_DTO project_dto);
        Task<IEnumerable<Project>> SearchProjectList(Project_DTO project_dto);

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


        public async Task<Project> InsertProject(Project_DTO project_dto)
        {
            try
            {
                Project project = new Project();
                var exsitingProject = _appDBContext.Project.Where(a => a.Project_Name == project_dto.Project_Name);
                if (exsitingProject.Count() > 0)
                {
                    project.Project_ID = 0;
                }
                else
                {
                    project.Project_Name = project_dto.Project_Name;
                    project.Status = (project_dto.isActive == true ? "Active" : "Inactive");
                    project.Created_Date = DateTime.Now;
                    project.Created_by = project_dto.Created_by;
                    project.Updated_by = project_dto.Created_by;
                    project.Updated_Date = DateTime.Now;
                    _appDBContext.Project.Add(project);
                    await _appDBContext.SaveChangesAsync();
                }
                return project;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<Project>> SearchProjectList(Project_DTO project_dto)
        {
            try
            {
                string projectName = project_dto.Project_Name;
                string status = (project_dto.isActive == true ? "Active" : "Inactive");

                if(!string.IsNullOrEmpty(projectName))
                return await _appDBContext.Project.Where(x => (x.Project_Name ?? "").Contains(projectName) && x.Status == status).ToListAsync();
                else
                    return await _appDBContext.Project.Where(x => x.Status == status).ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<Project> UpdateProject(Project_DTO project_dto)
        {
            try
            {
                Project project = new Project();
                project = _appDBContext.Project.Find(project_dto.Project_ID);
                project.Project_Name = project_dto.Project_Name;
                project.Updated_by = project_dto.Updated_by;
                project.Status = project_dto.isActive == true ? "Active" : "Inactive";
                project.Updated_Date = DateTime.Now;
                _appDBContext.Entry(project).State = EntityState.Modified;
                await _appDBContext.SaveChangesAsync();
                return project;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
