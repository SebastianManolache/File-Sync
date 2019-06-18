using System;

namespace ApiProject.Models.Dtos.File
{
    public class FileGet
    {
        public string Name { get; set; }
        public int Size { get; set; }
        public string Type { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
