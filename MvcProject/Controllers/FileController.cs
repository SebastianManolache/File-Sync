using ApiProject.Models.Dtos.File;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcProject.Controllers
{
    public class FileController : Controller
    {
        // GET: File
        public ActionResult Index()
        {
            return View("Index");
        }

        

        public ActionResult GetList(string BtnSubmit)
        {
            switch (BtnSubmit)
            {
                case "Get List":
                    var files = new List<FileGet>();
                    
                    return View("GetList",files);
                case "Cancel":
                    return RedirectToAction("Index");
            }
            return View("GetView");
            //return new EmptyResult();

        }
    }
}