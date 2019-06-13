using ApiProject.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiProject.Interfaces
{
    public interface IFileService
    {
        Task<List<File>> GetAsync();
        Task<File> CreateAsync(File file);
    }
}
