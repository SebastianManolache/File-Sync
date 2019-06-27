using ApiProject.Interfaces;
using Data.Models.Dtos.File;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiProject.Controllers
{
    [ApiController, Route("api/[controller]"), Produces("application/json")]
    public class FileController : ControllerBase
    {
        private readonly IFileService service;
        private readonly IMapper mapper;

        public FileController(IFileService service, IMapper mapper)
        {
            this.service = service;
            this.mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetFiles()
        {
            try
            {
                var result = service.GetFiles();

                if (result is null)
                {
                    return NotFound();
                }

                var currentfiles = mapper.Map<List<FileGet>>(result);
               
                return Ok(currentfiles);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpGet("byname/{localFileName}")]
        public IActionResult GetFileByName(string localFileName)
        {
            try
            {
                var result = service.GetByName(localFileName);

                if (result is null)
                {
                    return NotFound();
                }

                var currentfiles = mapper.Map<FilePost>(result);

                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpGet("download/{localFileName}")]
        public async Task<IActionResult> DownloadFile(string localFileName)
        {
            try
            {
                if (await service.DownloadFileAsync(localFileName))
                    return Ok();
                else
                    return NotFound();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        ///GET api/<controller>/5
        [HttpGet("upload/{localFileName}")]
        public async Task<IActionResult> UploadFile( string localFileName)
        {
            try
            {
                var result = await service.UploadFileAsync(localFileName);
                if (result is null)
                {
                    return NotFound();
                }

                return Ok();

            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpDelete("delete/{localFileName}")]
        public async Task<IActionResult> DeleteFile([FromRoute] string localFileName)
        {
            try
            {
                if (await service.DeleteAsync(localFileName))
                {
                    return Ok();
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
    }
}
