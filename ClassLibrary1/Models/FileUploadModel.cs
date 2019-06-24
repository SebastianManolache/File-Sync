using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Data.Models
{
    public class FileUploadModel:File
    {
        public HttpPostedFileBase fileUpload { get; set; }
    }
}
