using ApiProject.Controllers;
using ApiProject.Interfaces;
using AutoMapper;
using Data.Models.Dtos.File;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnitTestProject.FakeData;
using Xunit;

namespace UnitTestProject
{
    [Trait("Category", "# Player Controller")]
    public class FileControllerTest 
    {
        private readonly Mock<IFileService> mockFileManager;
        protected Mock<IMapper> MockMapper { get; }
        private readonly FileController controller;
        private readonly FileData fileData;

        public FileControllerTest()
        {
            mockFileManager = new Mock<IFileService>();
            MockMapper = new Mock<IMapper>();
            fileData = new FileData();
            controller = new FileController(
                               mockFileManager.Object,
                                MockMapper.Object);
        }

        [Trait("Category", "Get")]
        [Fact]
        //[FactName("Player Controller/Get -> players")]
        internal void GivenGetFilesCalledWhenDataExistThenReturnsData()
        {
            // Arrange
            var clubs = fileData.FakeFiles();
            var clubGets = fileData.FakeFileGets();

            mockFileManager
                .Setup(_ => _.GetFiles())
                .Returns(clubs);
            MockMapper
              .Setup(_ => _.Map<List<FileGet>>(clubs))
              .Returns(clubGets);

            // Act
            var response = controller.GetFiles();

            // Assert
            mockFileManager.Verify(_ => _.GetFiles(), Times.Once);
            MockMapper.Verify(_ => _.Map<List<FileGet>>(clubs), Times.Once());
            var result = Assert.IsType<OkObjectResult>(response);
            Assert.Same(clubGets, result.Value);
            Assert.True(result.StatusCode == 200);
        }

        [Trait("Category", "Get")]
        [Fact]
        internal void GivenGetFilesCalledWhenNoDataExistThenReturnsNoData()
        {
            // Arrange
            var files = fileData.FakeEmptyFiles();
            var fileGets = fileData.FakeEmptyFileGets();
            mockFileManager
                 .Setup(_ => _.GetFiles())
                 .Returns(files);
            MockMapper
              .Setup(_ => _.Map<List<FileGet>>(files))
              .Returns(fileGets);

            // Act
            var response = controller.GetFiles();

            // Assert
            mockFileManager.Verify(_ => _.GetFiles(), Times.Once);
           MockMapper.Verify(_ => _.Map<List<FileGet>>(files), Times.Never());
            var result = Assert.IsType<NotFoundResult>(response);
            Assert.True(result.StatusCode == 404);
        }

        [Trait("Category", "Get")]
        [Fact]
        internal void GivenGetFilesCalledWhenExceptionThrownThenHandlesGracefully()
        {
            // Arrange
            mockFileManager.
               Setup(_ => _.GetFiles())
               .Throws<Exception>();

            // Act
            var response = controller.GetFiles();

            // Assert
            mockFileManager.Verify(_ => _.GetFiles(), Times.Once);
            var result = Assert.IsType<BadRequestObjectResult>(response);
            Assert.True(result.StatusCode == 400);
        }

        ///
        [Trait("Category", "GetByName")]
        [Fact]
        //[FactName("Player Controller/Get -> players")]
        internal void GivenGetFileByNameCalledWhenDataExistThenReturnsData()
        {
            // Arrange
            var file = fileData.FakeFile();
            var filePost = fileData.FakeFilePost();
            var fakeLocalFileString = "test1.txt";

            mockFileManager
                .Setup(_ => _.GetByName(fakeLocalFileString))
                .Returns(file);
            MockMapper
              .Setup(_ => _.Map<FilePost>(file))
              .Returns(filePost);

            // Act
            var response = controller.GetFiles();

            // Assert
            mockFileManager.Verify(_ => _.GetByName(fakeLocalFileString), Times.Once);
            MockMapper.Verify(_ => _.Map<FilePost>(file), Times.Once());
            var result = Assert.IsType<OkObjectResult>(response);
            Assert.Same(filePost, result.Value);
            Assert.True(result.StatusCode == 200);
        }

        [Trait("Category", "GetByName")]
        [Fact]
        internal void GivenGetFileByNameCalledWhenNoDataExistThenReturnsNoData()
        {
            var fakeLocalFileString = "test1.txt";
            // Arrange
            var file = fileData.FakeFile();
            var filePost = fileData.FakeEmptyFilePost();
            mockFileManager
                 .Setup(_ => _.GetByName(fakeLocalFileString))
                 .Returns(file);
            MockMapper
              .Setup(_ => _.Map<FilePost>(file))
              .Returns(filePost);

            // Act
            var response = controller.GetFiles();

            // Assert
            mockFileManager.Verify(_ => _.GetFiles(), Times.Once);
            MockMapper.Verify(_ => _.Map<FilePost>(file), Times.Never());
            var result = Assert.IsType<NotFoundResult>(response);
            Assert.True(result.StatusCode == 404);
        }

        [Trait("Category", "GetByName")]
        [Fact]
        internal void GivenGetFileByNameCalledWhenExceptionThrownThenHandlesGracefully()
        {
            var fakeLocalFileString = "test1.txt";
            // Arrange
            mockFileManager.
               Setup(_ => _.GetByName(fakeLocalFileString))
               .Throws<Exception>();

            // Act
            var response = controller.GetFileByName(fakeLocalFileString);

            // Assert
            mockFileManager.Verify(_ => _.GetByName(fakeLocalFileString), Times.Once);
            var result = Assert.IsType<BadRequestObjectResult>(response);
            Assert.True(result.StatusCode == 400);
        }
        /// Delete
        /// 
        [Trait("Category", "delete")]
        [Fact]
        //[FactName("Player Controller/Get -> players")]
        internal async Task GivenDeleteCalledWhenDataExistThenReturnsDataAsync()
        {
            // Arrange
            var file = fileData.FakeFile();
            var fakeLocalFileString = "test1.txt";
            var boolResult = true;
            mockFileManager
                .Setup(_ => _.DeleteAsync(fakeLocalFileString))
                .ReturnsAsync(boolResult);
            

            // Act
            var response =await controller.DeleteFile(fakeLocalFileString);

            // Assert
            mockFileManager.Verify(_ => _.DeleteAsync(fakeLocalFileString), Times.Once);
            var result = Assert.IsType<OkResult>(response);
          // Assert.Equal(boolResult, result.Value);
            Assert.True(result.StatusCode == 200);
        }

        [Trait("Category", "delete")]
        [Fact]
        internal async Task GivenDeleteCalledWhenNoDataExistThenReturnsNoDataAsync()
        {
            var fakeLocalFileString = "test1.txt";
            // Arrange
            var boolResult = false;
            //var file = fileData.FakeEmptyFiles();
            mockFileManager
                 .Setup(_ => _.DeleteAsync(fakeLocalFileString))
                 .ReturnsAsync(boolResult);
            

            // Act
            var response =await controller.DeleteFile(fakeLocalFileString);

            // Assert
            mockFileManager.Verify(_ => _.DeleteAsync(fakeLocalFileString), Times.Once);
            var result = Assert.IsType<NotFoundResult>(response);
            Assert.True(result.StatusCode == 404);
        }

        [Trait("Category", "Delet")]
        [Fact]
        internal async Task GivenDeleteCalledWhenExceptionThrownThenHandlesGracefullyAsync()
        {
            var fakeLocalFileString = "test1.txt";
            // Arrange
            mockFileManager.
               Setup(_ => _.DeleteAsync(fakeLocalFileString))
               .Throws<Exception>();

            // Act
            var response =await controller.DeleteFile(fakeLocalFileString);

            // Assert
            mockFileManager.Verify(_ => _.DeleteAsync(fakeLocalFileString), Times.Once);
            var result = Assert.IsType<BadRequestObjectResult>(response);
            Assert.True(result.StatusCode == 400);
        }
    }
}
