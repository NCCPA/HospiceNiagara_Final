using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HospiceNiagara.Models;
using HospiceNiagara.Models.DatabaseModels;
using PagedList;

namespace HospiceNiagara.Controllers
{
    public class HospiceFilesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: HospiceFiles
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page, int? folderID)
        {
            SelectListItem allOption = new SelectListItem() { Value = "0", Text = "All" };
            SelectList folders = new SelectList(db.Folders.OrderBy(x => x.FolderName), "ID", "FolderName");

            var folderList = folders.ToList();
            folderList.Insert(0, allOption);



            ViewBag.FolderID = folderList;
            var files = db.Files.Include(f => f.Folder);

            //Set Sort Order
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";


            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            //Grab all Meetings
            var file = from m in db.Files
                       select m;

            //Filter
            if (!String.IsNullOrEmpty(searchString))
            {
                file = file.Where(m => m.FileName.Contains(searchString));
            }

            //Search by Folder
            if (folderID != null)
            {
                if (folderID != 0)
                {
                    file = file.Where(m => m.FolderID == folderID);
                }
            }

            //Switch to See what sorting we are going to do
            switch (sortOrder)
            {
                case "name_desc":
                    file = file.OrderByDescending(m => m.FileName);
                    break;
                case "Date":
                    file = file.OrderBy(m => m.Folder.FolderName);
                    break;
                case "date_desc":
                    file = file.OrderByDescending(m => m.Folder.FolderName);
                    break;
                default:
                    file = file.OrderBy(m => m.FileName);
                    break;
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);

            return View(file.ToPagedList(pageNumber, pageSize));
        }

        // GET: HospiceFiles/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            File file = db.Files.Find(id);
            if (file == null)
            {
                return HttpNotFound();
            }
            return View(file);
        }


        public FileContentResult FileDownload(int id)
        {
            var theFile = db.Files.Where(f => f.ID == id).SingleOrDefault();
            return File(theFile.FileContent, theFile.MimeType, theFile.FileName);
        }

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}
