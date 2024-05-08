using FileDetailAPI.Models;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
namespace FileDetailAPI.Repository
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<Employee_DTO>> GetEmployees();
        Task<Employee> GetEmployeeByID(int ID);
        Task<Employee> InsertEmployee(Employee objEmployee);
        Task<Employee> UpdateEmployee(Employee objEmployee);
        bool DeleteEmployee(int ID);
    }
    public class EmployeeRepository : IEmployeeRepository
    {

        private readonly APIDbContext _appDBContext;

        public EmployeeRepository(APIDbContext context)
        {
            _appDBContext = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<Employee_DTO>> GetEmployees()
        {
            var employeeList = await (from emp in _appDBContext.Employee
                                join dept in _appDBContext.Departments on emp.DepartmentId equals dept.DepartmentId

                                select new Employee_DTO
                                {
                                    EmployeeId = emp.EmployeeId,
                                    EmployeeName = emp.EmployeeName,
                                    DepartmentId =emp.DepartmentId,
                                    Department_DESC = dept.DepartmentName,
                                    DateOfJoining = emp.DateOfJoining,
                                    PhotoFileName = emp.PhotoFileName

                                }).ToListAsync();
            // return await _appDBContext.Employee.ToListAsync();

            return employeeList;
        }

        public async Task<Employee> GetEmployeeByID(int ID)
        {
            return await _appDBContext.Employee.FindAsync(ID);
        }

        public async Task<Employee> InsertEmployee(Employee objEmployee)
        {
            _appDBContext.Employee.Add(objEmployee);
            await _appDBContext.SaveChangesAsync();
            return objEmployee;
        }

        public async Task<Employee> UpdateEmployee(Employee objEmployee)
        {
            _appDBContext.Entry(objEmployee).State = EntityState.Modified;
            await _appDBContext.SaveChangesAsync();
            return objEmployee;
        }

        public bool DeleteEmployee(int ID)
        {
            bool result = false;
            var department = _appDBContext.Employee.Find(ID);
            if (department != null)
            {
                _appDBContext.Entry(department).State = EntityState.Deleted;
                _appDBContext.SaveChanges();
                result = true;
            }
            else
            {
                result = false;
            }
            return result;
        }
    }
}