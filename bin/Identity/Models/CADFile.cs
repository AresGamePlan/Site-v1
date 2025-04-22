using System;

namespace Identity.Models
{
    public class CADFile
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public DateTime UploadData { get; set; }
        public int IdProject { get; set; } 
    }
}
