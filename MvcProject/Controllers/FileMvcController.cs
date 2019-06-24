using Data.Interfaces;
using Data.Models;
using Data.Models.Dtos.File;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MvcProject.Controllers
{
    public class FileMvcController : Controller
    {
        public FileMvcController()
        {
        }
        private readonly IFileLayer fileLayer;

        public FileMvcController(IFileLayer fileLayer)
        {
            this.fileLayer = fileLayer;
        }

        // GET: File
        public ActionResult Index()
        {
            var list = new FileListModel();
            var files = new List<FileGet>();
           
            files = fileLayer.GetFiles();
            list.Files = files;

            return View("Index", list);
        }

        public ActionResult ActionFile(string BtnSubmit)
        {
            switch (BtnSubmit)
            {
                case "Upload File":
                    return View("Upload");
                case "Download":
                    return View("DownloadFile");
                case "Delete File":
                    return View("DeleteFile");
                case "Reset":
                    return RedirectToAction("Index");
            }
            return RedirectToAction("Index");

        }

        public ActionResult UploadFile(FileUploadModel fileUploadModel)
        {
            var csvreader = new StreamReader(fileUploadModel.fileUpload.InputStream);

            var name = fileUploadModel.fileUpload.FileName;
            name = Path.Combine(name, "");
            fileLayer.UploadFileAsync(name);
            //return View("UploadFile", fileUploadModel);
            return RedirectToAction("Index");

        }
        
        public async Task<ActionResult> DownloadFile(string file)
        {
            if (await fileLayer.DownloadFileAsync(file))
                return RedirectToAction("Index");

            var file1 = new FileUploadModel();
            file1.Name = file;
            return View("UploadFile", file1);

        }

        public ActionResult DeleteFile(string file)
        {
            fileLayer.DeleteAsync(file);
            return RedirectToAction("Index");
        }
    }
}