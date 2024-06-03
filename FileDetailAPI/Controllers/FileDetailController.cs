using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;
using System;
using FileDetailAPI.Models;
using FileDetailAPI.Repository;
using FileDetailAPI.LoggerManager;
using Microsoft.Extensions.Logging;

namespace FileDetailAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileDetailController : ControllerBase
    {
    //private readonly ILoggerManager _logger;
      private readonly ILogger<FileDetailController> _logger;
        //private readonly IWebHostEnvironment _env;
        private readonly IFileDetailsRepository _fileDetail;

        public FileDetailController(ILogger<FileDetailController> logger,IFileDetailsRepository fileDetail)
        {
            _logger = logger;    
            _fileDetail = fileDetail ?? throw new ArgumentNullException(nameof(fileDetail));
        }

        [HttpGet]
        [Route("GetFileList")]
        public async Task<IActionResult> Get()
        {
            
            try
            {
              _logger.LogInformation("Starting to Fetch File List Data.");
               var fileList = await _fileDetail.GetFileDetails();
              _logger.LogInformation("End to Fetch File List Data.");

             return Ok(fileList);
            }
            catch (Exception ex)
            {
              _logger.LogError($"Error occurred: {ex.Message}");
              _logger.LogError($"Stack Trace: {ex.StackTrace}");
              throw; 
            }
           
          }
        [HttpGet]
        [Route("SearchFileList")]
        public async Task<IActionResult> Get(string fileType,string projectName)
        {
          try
          {
            _logger.LogInformation("Starting To Call Search File List Data.");
            var fileList = await _fileDetail.SearchFileDetails(fileType, projectName);
            _logger.LogInformation("End To Call Search File List Data.");
            return Ok(fileList);
          }
          catch (Exception ex)
          {
            _logger.LogError($"Error occurred: {ex.Message}");
            _logger.LogError($"Stack Trace: {ex.StackTrace}");
              throw;
          }
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
              _logger.LogInformation("Starting To Call Download File");
              var result = await _fileDetail.DownloadFileById(userId,id);
              _logger.LogInformation("End To Call Download File");
              return File(result.FileData, "application/x-zip", result.FileName);

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred: {ex.Message}");
                _logger.LogError($"Stack Trace: {ex.StackTrace}");
                throw;
              //return  BadRequest(ex.Message);
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
                _logger.LogInformation("Starting To Call AddNewFile To Upload File");
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
                _logger.LogInformation("Ending To Call AddNewFile To Upload File");
                return new JsonResult("Upload File Successfully");

            }
            catch (Exception ex)
            {
              _logger.LogError($"Error occurred: {ex.Message}");
              _logger.LogError($"Stack Trace: {ex.StackTrace}");
              throw;
              // return BadRequest(ex.Message);
            }
        }
        [HttpDelete]
        [Route("DeleteFile/{Id}")]
        public JsonResult Delete(int Id)
        {
            try
            {
              _logger.LogInformation("Starting To Call Delete  File and file Id ="+Id.ToString());
              var result = _fileDetail.DeleteFile(Id);
              _logger.LogInformation("Ending To Call Delete  File and file Id =" + Id.ToString());
              return new JsonResult("Deleted File Successfully");
            }
            catch (Exception ex)
            {
                  _logger.LogError($"Error occurred: {ex.Message}");
                  _logger.LogError($"Stack Trace: {ex.StackTrace}");
                  throw;
                //return new JsonResult(ex.Message);
            }
        }
        [HttpGet("error")]
        public IActionResult GetError()
        {
          throw new Exception("Test exception");
        }
  }
}
