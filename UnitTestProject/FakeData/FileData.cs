using Data.Models;
using Data.Models.Dtos.File;
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
                Name = "test1.txt",
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
                Name = "test1.txt",
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

        public List<File> FakeEmptyFiles()
        {
            var files = new List<File>();

            return files;
        }

        public List<FileGet> FakeFileGets()
        {
            return new List<FileGet>()
            {
                new FileGet
                {
                Name = "AAA",
                Size = 23,
                CreatedAt=DateTime.UtcNow,
                Type="txt"
                    },
                new FileGet
                {
                Name = "AAA",
                Size = 23,
                CreatedAt=DateTime.UtcNow,
                Type="txt"
                }
            };
        }

        public FileGet FakeFileGet()
        {
            return new FileGet
            {
                Name = "AAA",
                Size = 23,
                CreatedAt = DateTime.UtcNow,
                Type = "txt"
            };
        }
        public FilePost FakeFilePost()
        {
            return new FilePost
            {
                Name = "test1.txt",
                Size = 23,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Type = "txt"
            };
        }

        public List<FileGet> FakeEmptyFileGets()
        {
            //return new List<FileGet>()
            //{
            //    new FileGet
            //    {
            //    Name = "AAA",
            //    Size = 23,
            //    CreatedAt=DateTime.UtcNow,
            //    Type="txt"
            //        },
            //    new FileGet
            //    {
            //    Name = "AAA",
            //    Size = 23,
            //    CreatedAt=DateTime.UtcNow,
            //    Type="txt"
            //    }
            //};
            var files = new List<FileGet>();

            return files;
        }
        public FilePost FakeEmptyFilePost()
        {
            return new FilePost();
        }

    }
}
