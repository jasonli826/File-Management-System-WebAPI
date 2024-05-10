using FileDetailAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FileDetailAPI.Models;
using FileDetailAPI.Repository;
using System.Globalization;
using Microsoft.Data.SqlClient;
using System.IO.Compression;
using System.Reflection.Metadata.Ecma335;

namespace FileDetailAPI.Repository
{
    public interface IFileDetailsRepository
    {
        Task<IEnumerable<FileDetails_DTO>> GetFileDetails();
        Task<IEnumerable<FileDetails_DTO>> SearchFileDetails(string fileType,string projectName);
        Task<FileDetails> CreateNewFile(IFormFile file,UploadData uploadData);
        Task<FileDetails> DownloadFileById(int Id);
        bool DeleteFile(int ID);
    }
    public class FileDetailsRepository : IFileDetailsRepository
    {
        private readonly APIDbContext _appDBContext;

        public FileDetailsRepository(APIDbContext context)
        {
            _appDBContext = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<FileDetails_DTO>> GetFileDetails()
        {
           List<FileDetails_DTO> fileList = null;
            try
            {
                 fileList = await (from fd in _appDBContext.FileDetails


                                      select new FileDetails_DTO
                                      {
                                          Id = fd.Id,
                                          FileName = fd.FileName,
                                          FileSize = fd.FileSize,
                                          FileType = fd.FileType,
                                          VersionNo = fd.VersionNo,
                                          UploadBy = fd.UploadBy,
                                          ReleaseDate = fd.ReleaseDate.Date,
                                          ReleaseNotes = fd.ReleaseNotes,
                                          UploadDateTime = fd.UploadDateTime,
                                          ProjectName = fd.Project_Name

                                      }).AsQueryable().OrderByDescending(x => x.UploadDateTime).ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return fileList;
        }
        public async Task<FileDetails> CreateNewFile(IFormFile file, UploadData uploadData)
        {
            FileDetails fileDetails = null;
            try
            {
                fileDetails = new FileDetails()
                {
                    Id = 0,
                    FileName = file.FileName,
                    VersionNo = uploadData.versionNo,
                    ReleaseDate = uploadData.dateOfReleasse,
                    ReleaseNotes = uploadData.releaseContent,
                    FileType = uploadData.uploadType,
                    UploadBy = uploadData.releaseBy,
                    UploadDateTime = DateTime.Now,
                    Project_Name = uploadData.projectName,
                    FileExtension = ".ZIP",
                    FileSize = (Math.Round((double)file.Length / 1048576, 2)).ToString() + " M"
                   
                };

                using (var stream = new MemoryStream())
                {
                    file.CopyTo(stream);
                    fileDetails.FileData = CompressData(stream.ToArray());
                }

                var result = _appDBContext.FileDetails.Add(fileDetails);
                await _appDBContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return fileDetails;

        }
        public async Task<FileDetails> DownloadFileById(int Id)
        {
            FileDetails file = null;
            try
            {
                var result = _appDBContext.FileDetails.AsNoTracking().ToList().Where(x=>x.Id==Id).FirstOrDefault();
                result.FileData = DecompressData(result.FileData);
                file = result;
                return file;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task CopyStream(Stream stream, string downloadPath)
        {
            using (var fileStream = new FileStream(downloadPath, FileMode.Create, FileAccess.Write))
            {
                await stream.CopyToAsync(fileStream);
            }
        }
        private byte[] CompressData(byte[] data)
        {
            using (MemoryStream compressedStream = new MemoryStream())
            {
                using (DeflateStream compressionStream = new DeflateStream(compressedStream, CompressionLevel.SmallestSize))
                {
                    compressionStream.Write(data, 0, data.Length);
                }
                return compressedStream.ToArray();
            }
        }
        private byte[] DecompressData(byte[] compressedData)
        {
            using (MemoryStream compressedStream = new MemoryStream(compressedData))
            {
                using (MemoryStream decompressedStream = new MemoryStream())
                {
                    using (DeflateStream decompressionStream = new DeflateStream(compressedStream, CompressionMode.Decompress))
                    {
                        decompressionStream.CopyTo(decompressedStream);
                    }
                    return decompressedStream.ToArray();
                }
            }
        }

        public bool DeleteFile(int ID)
        {
            try
            {
                bool result = false;
                var file = _appDBContext.FileDetails.Find(ID);
                if (file != null)
                {
                    _appDBContext.Entry(file).State = EntityState.Deleted;
                    _appDBContext.SaveChanges();
                    result = true;
                }
                else
                {
                    result = false;
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<FileDetails_DTO>> SearchFileDetails(string fileType, string projectName)
        {
            List<FileDetails_DTO> searchFileList = null;
            try
            {
                if (string.IsNullOrEmpty(fileType) || string.IsNullOrEmpty(projectName))
                {
                    searchFileList = await (from fd in _appDBContext.FileDetails
                                            select new FileDetails_DTO
                                            {
                                                Id = fd.Id,
                                                FileName = fd.FileName,
                                                FileSize = fd.FileSize,
                                                FileType = fd.FileType,
                                                VersionNo = fd.VersionNo,
                                                UploadBy = fd.UploadBy,
                                                ReleaseDate = fd.ReleaseDate.Date,
                                                ReleaseNotes = fd.ReleaseNotes,
                                                UploadDateTime = fd.UploadDateTime,
                                                ProjectName = fd.Project_Name

                                            }).Where(x => (x.FileType ?? "").Contains(fileType) || (x.ProjectName ?? "").Contains(projectName)).AsQueryable().OrderByDescending(x => x.UploadDateTime).ToListAsync();

                }
                else
                {
                    searchFileList = await (from fd in _appDBContext.FileDetails
                                            select new FileDetails_DTO
                                            {
                                                Id = fd.Id,
                                                FileName = fd.FileName,
                                                FileSize = fd.FileSize,
                                                FileType = fd.FileType,
                                                VersionNo = fd.VersionNo,
                                                UploadBy = fd.UploadBy,
                                                ReleaseDate = fd.ReleaseDate.Date,
                                                ReleaseNotes = fd.ReleaseNotes,
                                                UploadDateTime = fd.UploadDateTime,
                                                ProjectName = fd.Project_Name

                                            }).Where(x => (x.FileType ?? "").Contains(fileType) && (x.ProjectName ?? "").Contains(projectName)).AsQueryable().OrderByDescending(x => x.UploadDateTime).ToListAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return searchFileList;
        }
    }
    
}
