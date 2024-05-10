using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace FileDetailAPI.Models
{

    public class FileDetails_DTO
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string FileName { get; set; }
        [Required]
        public string FileType { get; set; }
        [Required]
        public string FileSize { get; set; }

        [Required]
        public string VersionNo { get; set; }

        [Required]
        public DateTime ReleaseDate { get; set; }

        public string ReleaseNotes { get; set; }

        [Required]
        public string UploadBy { get; set; }

        [Required]
        public DateTime UploadDateTime { get; set; }
        [Required]
        public string ProjectName { get; set; }

    }
}
