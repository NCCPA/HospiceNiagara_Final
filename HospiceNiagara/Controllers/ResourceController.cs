using HospiceNiagara.Models;
using HospiceNiagara.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HospiceNiagara.Controllers
{
    public class ResourceController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Resource
        public ActionResult Index()
        {
            var folderList = from x in db.Folders.Include("Files")
                             select new FolderViewModel
                             {
                                 ID = x.ID,
                                 FolderName = x.FolderName,
                                 FolderDescription = x.FolderDescription ?? "N/A",
                                 Files = x.Files
                             };

            return View(folderList);
        }

        public FileContentResult Download(int id)
        {
            var theFile = db.Files.Where(f => f.ID == id).SingleOrDefault();
            return File(theFile.FileContent, theFile.MimeType, theFile.FileName);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}