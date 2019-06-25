using System;

namespace Data.Models.Dtos.File
{
    public class FileGet
    {
        public string Name { get; set; }
        public int Size { get; set; }
        public string Type { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool isSelected { get; set; }
    }
}
