using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using FileDetailAPI.Models;

namespace FileDetailAPI.Repository
{
    public interface IEmployeeAngularRepository
    {
        Task<IEnumerable<EmployeeAngular>> GetEmployees();
        Task<EmployeeAngular> GetEmployeeByID(int ID);
        Task<EmployeeAngular> InsertEmployee(EmployeeAngular objEmployee);
        Task<EmployeeAngular> UpdateEmployee(EmployeeAngular objEmployee);
        bool DeleteEmployee(int ID);
    }


    public class EmployeeAngularRepository : IEmployeeAngularRepository
    {
        private readonly APIDbContext _appDBContext;

        public EmployeeAngularRepository(APIDbContext context)
        {
            _appDBContext = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<EmployeeAngular>> GetEmployees()
        {
          //  return await _appDBContext.EmployeeAngular.ToListAsync();
            return await _appDBContext.EmployeeAngular.FromSqlRaw<EmployeeAngular>("GetEmployees").ToListAsync();
        }

        public async Task<EmployeeAngular> GetEmployeeByID(int ID)
        {
            // return await _appDBContext.EmployeeAngular.FindAsync(ID);
            return (await _appDBContext.EmployeeAngular.FromSqlRaw<EmployeeAngular>("GetEmployeesByID @EmployeeID", new SqlParameter("@EmployeeID", ID)).ToListAsync()).FirstOrDefault(); 
        }

        public async Task<EmployeeAngular> InsertEmployee(EmployeeAngular objEmployee)
        {
            var employId = new SqlParameter()
            {
                ParameterName = "EmployeeID",
                SqlDbType = System.Data.SqlDbType.Int,
                Direction = System.Data.ParameterDirection.Output
            };
            //_appDBContext.EmployeeAngular.Add(objEmployee);
            //await _appDBContext.SaveChangesAsync();
            //return objEmployee;
            int i =  await _appDBContext.Database.ExecuteSqlRawAsync("AddEmployee @EmployeeName,@Department,@EmailId,@DateOfJoining,@PhotoFileName,@EmployeeID OUTPUT",
                    new SqlParameter("@EmployeeName", objEmployee.EmployeeName),
                    new SqlParameter("@Department", objEmployee.Department),
                    new SqlParameter("@EmailId", objEmployee.EmailId),
                    new SqlParameter("@DateOfJoining", objEmployee.DateOfJoining),
                    new SqlParameter("@PhotoFileName", objEmployee.PhotoFileName),
                    employId);

            objEmployee.EmployeeID = (int)employId.Value;
            return objEmployee;
        }

        public async Task<EmployeeAngular> UpdateEmployee(EmployeeAngular objEmployee)
        {
            //_appDBContext.Entry(objEmployee).State = EntityState.Modified;
            //await _appDBContext.SaveChangesAsync();
            try
            {
                await _appDBContext.Database.ExecuteSqlRawAsync("UpdateEmployee @EmployeeID,@EmployeeName,@Department,@EmailId,@DateOfJoining,@PhotoFileName",
                   new SqlParameter("@EmployeeID", objEmployee.EmployeeID),
                   new SqlParameter("@EmployeeName", objEmployee.EmployeeName),
                   new SqlParameter("@Department", objEmployee.Department),
                   new SqlParameter("@EmailId", (objEmployee.EmailId==null?string.Empty: objEmployee.EmailId)),
                   new SqlParameter("@DateOfJoining", objEmployee.DateOfJoining),
                   new SqlParameter("@PhotoFileName", (objEmployee.PhotoFileName==null?string.Empty: objEmployee.PhotoFileName)));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return objEmployee;
        }

        public bool DeleteEmployee(int ID)
        {
            bool result = false;
            var department = _appDBContext.EmployeeAngular.Find(ID);
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

