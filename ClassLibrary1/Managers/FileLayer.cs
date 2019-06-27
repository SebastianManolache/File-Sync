using Data.Interfaces;
using Data.Models;
using Data.Models.Dtos.File;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using File = Data.Models.File;
using Microsoft.Data.Entity;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace Data.Managers
{
    public class FileLayer : IFileLayer
    {

        public async Task<List<FileGet>> GetFiles()
        {
            string Baseurl = "https://localhost:44305";
            var fileList = new List<FileGet>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage res = await client.GetAsync("/api/file");
                if (res.IsSuccessStatusCode)
                {
                    var empresponse = res.Content.ReadAsStringAsync().Result;
                    fileList = JsonConvert.DeserializeObject<List<FileGet>>(empresponse);
                }

                return fileList;
            }
        }

        public async Task<File> UploadFileAsync(string fileName)
        {
            string Baseurl = "https://localhost:44305";
            var file = new File();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage res = await client.GetAsync($"/api/file/upload/{fileName}");
                if (res.IsSuccessStatusCode)
                {
                    var empresponse = res.Content.ReadAsStringAsync().Result;
                    file = JsonConvert.DeserializeObject<File>(empresponse);
                }

                return file;
            }
        }

        public async Task<File> GetByName(string fileName)
        {
            string Baseurl = "https://localhost:44305";
            var file = new File();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage res = await client.GetAsync($"/api/file/byname/{fileName}");

                if (res.IsSuccessStatusCode)
                {
                    var empresponse = res.Content.ReadAsStringAsync().Result;
                    file = JsonConvert.DeserializeObject<File>(empresponse);
                }

                return file;
            }
        }

        public async Task<bool> DeleteAsync(string localFileName)
        {
            string Baseurl = "https://localhost:44305";
            var file = new File();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage res = await client.DeleteAsync($"/api/file/delete/{localFileName}");

                if (res.IsSuccessStatusCode)
                {
                    return true;
                }

                return false;
            }
        }

        public async Task<bool> DownloadFileAsync(string localFileName)
        {
            string Baseurl = "https://localhost:44305";
            var file = new File();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage res = await client.GetAsync($"/api/file/download/{localFileName}");

                if (res.IsSuccessStatusCode)
                {
                    var empresponse = res.Content.ReadAsStringAsync().Result;
                    file = JsonConvert.DeserializeObject<File>(empresponse);

                    return true;
                }

                return true;
            }
        }
    }
}
