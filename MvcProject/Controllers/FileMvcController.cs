using Data.Interfaces;
using Data.Models;
using Data.Models.Dtos.File;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        private List<FileGet> listFileSelected;

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
                    return RedirectToAction("SelectedFile");
                //var list = SelectedFile();
                //return DeleteFile();
                case "Selected File":
                    return RedirectToAction("SelectedFile");
                //return View("DeleteFile");
                case "Reset":
                    return RedirectToAction("Index");
                case "Sync":
                    return RedirectToAction("SyncFile");
                case "Sort Files":
                    return View("Sort");
                    //return RedirectToAction("SortFile");


            }
            return RedirectToAction("Index");

        }

        public ActionResult UploadFile(FileUploadModel fileUploadModel)
        {
            var csvreader = new StreamReader(fileUploadModel.fileUpload.InputStream);

            var name = fileUploadModel.fileUpload.FileName;
            //name = Path.GetFileName(name);
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

        //public ActionResult DeleteFile(string file)
        //{

        //    fileLayer.DeleteAsync(file);
        //    return RedirectToAction("Index");
        //}

        public ActionResult DeleteFile()
        {
            return View("UploadFile", listFileSelected);
            this.listFileSelected.ForEach(file =>
            {
                if (file.isSelected == true)
                    fileLayer.DeleteAsync(file.Name);
            });
            return RedirectToAction("Index");

        }

        public ActionResult SelectedFile(FileListModel listFile)
        {
            // var listFileSelected = fileLayer.GetFiles();
            var listFileSelected = new List<FileGet>();
            listFile.Files.ForEach(file =>
            {
                if (file.isSelected == true)
                {
                    listFileSelected.Add(file);
                }
            });
            this.listFileSelected = listFileSelected;
            return View("UploadFile", listFileSelected);
            //return listFileSelected;
        }

        public ActionResult SyncFile()
        {
            var files = fileLayer.GetFiles();
            files.ForEach(file =>
            {
                fileLayer.DownloadFileAsync(file.Name);
            });
            return RedirectToAction("Index");
        }

        public ActionResult SortFile(string BtnSubmit)
        {
            var list = fileLayer.GetFiles();
            var fileListModel = new FileListModel();
            //var currentList = new List<FileGet>();
            switch (BtnSubmit)
            {
                case "Name":
                    var currentList = list.OrderBy(file => file.Name).ToList();
                    fileListModel.Files = currentList;
                    return View("Index", fileListModel);
                case "Size":
                    currentList = list.OrderBy(file => file.Size).ToList();
                    fileListModel.Files = currentList;
                    return View("Index", fileListModel);
            }
            return RedirectToAction("Index");

            // var list = fileLayer.GetFiles();
            // //var currentList = new List<FileGet>();
            // var fileListModel = new FileListModel();


            //var currentList = list.OrderBy(file  => file.Name).ToList();
            // fileListModel.Files = currentList;
            // return View("Index", fileListModel);


        }
    }
}