using ApiProject.Interfaces;
using ApiProject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiProject.Services
{
    public class FileService:IFileService
    {
        public async Task<List<File>> GetAsync()
        {
            using (var db = new FileDbContext())
            {
                var files = await db.File.ToListAsync();
                return files;
            }
        }

        public async Task<File> CreateAsync(File file)
        {
            using (var db = new FileDbContext())
            {
                await db.File.AddAsync(file);
                await db.SaveChangesAsync();
                return file;
            }
        }
    }
}
