using Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC.Controllers
{
    public class FilesController : Controller
    {
        private IFileServices _services;

        public FilesController(IFileServices services)
        {
            _services = services;
        }

        public ActionResult DownloadFile(int id)
        {
            var file = _services.GetFile(id);

            if (file != null)
            {
                return File(file.FileContent, file.ContentType);
            }
            return View();
        }
    }
}