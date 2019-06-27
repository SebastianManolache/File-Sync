using Data.Models;
using System;
using System.Collections.Generic;

namespace XUnitTestFile.FakeData
{
    class FakeDataFile
    {
        public File FakeFile()
        {
            return new File
            {
                Id = 1,
                Name = "adg",
                Size = 3,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Type = "png"
            };
        }

        public List<File> FakeEmptyFiles()
        {
            var files = new List<File>();

            return files;
        }
        public List<File> FakeFileGets()
        {
            return new List<File>()
            {
                new File
                {
                    Id = 2,
                    Name = "AAA",
                    Size=23,
                    CreatedAt= DateTime.UtcNow,
                    UpdatedAt=  DateTime.UtcNow,
                    Type="txt"
                    },
                new File
                {
                    Id = 5,
                    Name = "ccc",
                    Size=243,
                    CreatedAt= DateTime.UtcNow,
                    UpdatedAt=  DateTime.UtcNow,
                    Type="ppt"
                }
            };
        }
    }
}
