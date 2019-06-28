using Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTestProject.FakeData
{
    public class FileData : File
    {
        public File FakeFile()
        {
            return new File
            {
                Id = 2,
                Name = "AAA",
                Size = 23,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Type = "txt"
            };
        }

        public List<File> FakeFiles()
        {
            return new List<File>()
            {
                new File
                {
                   Id = 2,
                Name = "AAA",
                Size = 23,
                CreatedAt=DateTime.UtcNow,
                UpdatedAt=DateTime.UtcNow,
                Type="txt"
                    },
                new File
                {
                    Id = 2,
                Name = "AAA",
                Size = 23,
                CreatedAt=DateTime.UtcNow,
                UpdatedAt=DateTime.UtcNow,
                Type="txt"
                }
            };
        }
    }
}
