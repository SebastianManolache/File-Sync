using Data.Interfaces;
using Data.Models;
using Data.Models.Dtos.File;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Http;

namespace MvcProject.Controllers
{
    public class FileMvcController : Controller
    {
        private readonly IFileLayer fileLayer;
        private List<FileGet> listFileSelected;

        public FileMvcController(IFileLayer fileLayer)
        {
            this.fileLayer = fileLayer;
        }

        // GET: File
        public async Task<ActionResult> Index()
        {
            try
            {
                var files = await fileLayer.GetFiles();

                return View("IndexNew", files);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public ActionResult ActionFile(string BtnSubmit)
        {
            try
            {
                switch (BtnSubmit)
                {
                    case "Upload File":
                        return View("Upload");
                    case "Download":
                        return View("DownloadFile");
                    case "Delete File":
                        return View("DeleteFile");
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
                    case "DeleteFilesSelected":
                        return RedirectToAction("DeleteSelected");

                }

                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public ActionResult UploadFile(FileUploadModel fileUploadModel)
        {
            try
            {
                var csvreader = new StreamReader(fileUploadModel.fileUpload.InputStream);

                var name = fileUploadModel.fileUpload.FileName;

                name = Path.Combine(name, "");
                fileLayer.UploadFileAsync(name.ToString());

                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<ActionResult> DownloadFile(string file)
        {
            try
            {
                if (await fileLayer.DownloadFileAsync(file))
                    return RedirectToAction("Index");

                var file1 = new FileUploadModel();
                file1.Name = file;
                return View("UploadFile", file1);

            }
            catch (Exception e)
            {
                return null;
            }
        }

        public ActionResult DeleteFile(string file)
        {
            try
            {
                fileLayer.DeleteAsync(file);
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                return null;
            }
        }

        //public ActionResult DeleteFile()
        //{
        //    //return View("UploadFile", listFileSelected);
        //    this.listFileSelected.ForEach(file =>
        //    {
        //        if (file.isSelected == true)
        //            fileLayer.DeleteAsync(file.Name);
        //    });
        //    return RedirectToAction("Index");

        //}

        public ActionResult SelectedFile(FileListModel listFile)
        {
            try
            {
                //var listFileSelected = fileLayer.GetFiles();
                //var listFileSelected = new List<FileGet>();
                listFile.Files.ForEach(file =>
                {
                    if (file.isSelected == true)
                    {
                        listFileSelected.Add(file);
                    }
                });
                return View("UploadFile", listFileSelected);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<ActionResult> SyncFile()
        {
            try
            {
                var files = await fileLayer.GetFiles();
                files.ForEach(file =>
                {
                    fileLayer.DownloadFileAsync(file.Name);
                });
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<ActionResult> SortFile(string BtnSubmit)
        {
            try
            {
                var list = await fileLayer.GetFiles();

                switch (BtnSubmit)
                {
                    case "Name":
                        var currentList = list.OrderBy(file => file.Name).ToList();
                        return View("IndexNew", currentList);
                    case "Size":
                        currentList = list.OrderBy(file => file.Size).ToList();
                        return View("IndexNew", currentList);
                    case "CreateData":
                        currentList = list.OrderBy(file => file.CreatedAt).ToList();
                        return View("IndexNew", currentList);
                }

                return RedirectToAction("IndexNew");
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<ActionResult> DeleteSelected()
        {
            try
            {
                var list = await fileLayer.GetFiles();
                list.ForEach(file =>
                {
                    if (file.isSelected == true)
                        fileLayer.DeleteAsync(file.Name);
                });
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                return null;
            }
        }

    }
}