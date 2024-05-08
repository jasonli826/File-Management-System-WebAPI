using System;

namespace FileDetailAPI.Models
{
    public class UploadData
    {
        public string versionNo { get; set; }
        public string releaseBy { get; set; }
        public string uploadType { get; set; }
        public DateTime dateOfReleasse { get; set; }
        public string releaseContent { get; set; }

        public string uploadBy { get; set; }
    }
}
