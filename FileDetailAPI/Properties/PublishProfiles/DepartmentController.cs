﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using FileDetailAPI.Models;
using FileDetailAPI.Repository;

namespace FileDetailAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentRepository _department;

        public DepartmentController(IDepartmentRepository department)
        {
            _department = department ?? throw new ArgumentNullException(nameof(department));
        }

        [HttpGet]
        [Route("GetDepartment")]
        public async Task<IActionResult> Get()
        {
            return Ok(await _department.GetDepartment());
        }

        [HttpGet]
        [Route("GetDepartmentByID/{Id}")]
        public async Task<IActionResult> GetDeptById(int Id)
        {
            return Ok(await _department.GetDepartmentByID(Id));
        }

        [HttpPost]
        [Route("AddDepartment")]
        public async Task<IActionResult> Post(Department dep)
        {
            var result = await _department.InsertDepartment(dep);
            if (result.DepartmentId == 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Something Went Wrong");
            }
            else if (result.DepartmentId == 500)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Department Name is duplicated");
            }
            return new JsonResult("Added Department Successfully");
        }

        [HttpPut]
        [Route("UpdateDepartment")]
        public async Task<IActionResult> Put(Department dep)
        {
            await _department.UpdateDepartment(dep);
            //return Ok("Updated Successfully");
            return new JsonResult("Updated Department Successfully");
        }


        [HttpDelete]
        [Route("DeleteDepartment/{Id}")]
        public JsonResult Delete(int id)
        {
            _department.DeleteDepartment(id);
            return new JsonResult("Deleted Successfully");
        }
    }
}
