using Microsoft.VisualBasic.FileIO;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FileDetailAPI.Models
{

    [Table("FileDetails")]
     public class FileDetails
    {
        [Key]
        public int Id { get; set; }

        public string FileName { get; set; }

        public string FileType { get; set; }

        public byte[] FileData { get; set; }

        public string VersionNo { get; set; }


        public DateTime ReleaseDate { get; set; }
     
        public string ReleaseNotes { get; set; }

        public string ReleaseBy { get; set; }
        public string UploadBy { get; set; }

        public string FileSize { get; set; }

        public string FileExtension { get; set; }
        public DateTime UploadDateTime { get; set; }
        public override string ToString()
        {
            return FileName;
        }
    }
}
