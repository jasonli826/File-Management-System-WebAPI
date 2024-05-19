using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;
using System;
using FileDetailAPI.Models;
using FileDetailAPI.Repository;
using Microsoft.VisualBasic.FileIO;
using FileDetailAPI.Models;

namespace FileDetailAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileDetailController : ControllerBase
    {
        //private readonly IWebHostEnvironment _env;
        private readonly IFileDetailsRepository _fileDetail;

        public FileDetailController( IFileDetailsRepository fileDetail)
        {
            _fileDetail = fileDetail ?? throw new ArgumentNullException(nameof(fileDetail));
        }

        [HttpGet]
        [Route("GetFileList")]
        public async Task<IActionResult> Get()
        {
            return Ok(await _fileDetail.GetFileDetails());
        }
        [HttpGet]
        [Route("SearchFileList")]
        public async Task<IActionResult> Get(string fileType,string projectName)
        {
            return Ok(await _fileDetail.SearchFileDetails(fileType,projectName));
        }
        [HttpGet("DownloadFile")]
        [RequestSizeLimit(1073741824)]
        public async Task<ActionResult> DownloadFile(string userId,int id)
        {
            if (id < 1)

            {
                return BadRequest();
            }

            try
            {
               var result = await _fileDetail.DownloadFileById(userId,id);
               return File(result.FileData, "application/x-zip", result.FileName);
            }
            catch (Exception ex)
            {
               return  BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("AddNewFile")]
        [RequestSizeLimit(2147483648)]
        public async Task<IActionResult> Post( IFormFile fileDetails,string version,string uploadType,string releaseBy,DateTime releaseDate,string releaseContent,string uploadBy,string projectName)
        {
            UploadData uploadData = new UploadData();
            uploadData.uploadType = uploadType;
            uploadData.versionNo = version;
            uploadData.releaseBy = releaseBy;
            uploadData.dateOfReleasse = releaseDate;
            uploadData.releaseContent = releaseContent;
            uploadData.uploadBy = uploadBy;
            uploadData.projectName = projectName;
            string extension = string.Empty;
            if (fileDetails == null)
            {
                return BadRequest();
            }
            try
            {

                extension = System.IO.Path.GetExtension(fileDetails.FileName);

                if (extension.ToLower() != ".zip")
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Please Upload Zip File");
                }

                var result = await _fileDetail.CreateNewFile(fileDetails,uploadData);
                if (result == null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Something Went Wrong");
                }

                return new JsonResult("Upload File Successfully");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete]
        [Route("DeleteFile/{Id}")]
        public JsonResult Delete(int Id)
        {
            try
            {
                var result = _fileDetail.DeleteFile(Id);
                return new JsonResult("Deleted File Successfully");
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }

        //[HttpPut]
        //[Route("UpdateEmployee")]
        //public async Task<IActionResult> Put(Employee emp)
        //{
        //    //var result = await _employee.UpdateEmployee(emp);
        //    //if (result == null)
        //    //{
        //    //    return StatusCode(StatusCodes.Status500InternalServerError, "Something Went Wrong");
        //    //}
        //    //return Ok("Updated Successfully");
        //    await _employee.UpdateEmployee(emp);
        //    return Ok("Updated Successfully");
        //}


        //[HttpDelete("{id}")]
        //public JsonResult Delete(int id)
        //{
        //    var result = _employee.DeleteEmployee(id);
        //    return new JsonResult("Deleted Successfully");
        //}


        //[Route("SaveFile")]
        //[HttpPost]
        //public JsonResult SaveFile()
        //{
        //    try
        //    {
        //        var httpRequest = Request.Form;
        //        var postedFile = httpRequest.Files[0];
        //        string filename = postedFile.FileName;
        //        var physicalPath = _env.ContentRootPath + "/Photos/" + filename;

        //        using (var stream = new FileStream(physicalPath, FileMode.Create))
        //        {
        //            postedFile.CopyTo(stream);
        //        }

        //        return new JsonResult(filename);
        //    }
        //    catch (Exception)
        //    {
        //        return new JsonResult("anonymous.png");
        //    }
        //}


        //[Route("GetAllDepartmentNames")]
        //[HttpGet]
        //public async Task<IActionResult> GetAllDepartmentNames()
        //{
        //    return Ok(await _department.GetDepartment());
        //}
    }
}
