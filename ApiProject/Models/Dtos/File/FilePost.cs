using System;

namespace ApiProject.Models.Dtos.File
{
    public class FilePost
    {
        public string Name { get; set; }
        public long Size { get; set; }
        public string Type { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
