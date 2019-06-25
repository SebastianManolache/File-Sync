using Data.Models.Dtos.File;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class FileListModel
    {
        public List<FileGet> Files { get; set; }
        public bool IsSelectedAll { get; set; }
    }
}
