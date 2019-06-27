using Moq;
using MvcProject.Controllers;
using System;
using Xunit;
using AutoMapper;
using XUnitTestFile.FakeData;
using System.Threading.Tasks;
using ApiProject.Controllers;
using ApiProject.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Data.Models.Dtos.File;
using System.Collections.Generic;

namespace TestingFile
{
    [Trait("Category", "# FileMvc Controller")]
    public class FileTesting
    {
        private readonly Mock<IFileService> mockFileManager;
        protected Mock<IMapper> MockMapper { get; }
        private readonly FileController controller;
        private readonly FakeDataFile fileData;

        public FileTesting()
        {
            mockFileManager = new Mock<IFileService>();
            MockMapper = new Mock<IMapper>();
            controller = new FileController(mockFileManager.Object,MockMapper.Object);
                               
        }

        [Trait("Category", "Get")]
        [Fact]
        internal void GivenGetCalledWhenDataExistThenReturnsData()
        {
            // Arrange
            var files = fileData.FakeFile();
            var filesGets = fileData.FakeFileGets();
            //mockFileManager.
            //    Setup(_ => _.GetFiles())
            //    .Returns(files);


            // Act
            // var response = await controller.GetFiles();

            // Assert
            mockFileManager.Verify(_ => _.GetFiles(), Times.Once);
            MockMapper.Verify(_ => _.Map<List<FileGet>>(files), Times.Once());
            //    var result = Assert.IsType<OkObjectResult>(response);
            //    Assert.Same(filesGets, result.Value);
            //    Assert.True(result.StatusCode == 200);
        }


    }
}

