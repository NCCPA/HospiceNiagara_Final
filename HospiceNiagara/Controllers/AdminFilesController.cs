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
using System.IO;
using System.Data.Entity.Validation;
using System.Text;
using PagedList;

namespace HospiceNiagara.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminFilesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: AdminFiles2
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
            if(folderID != null)
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

        // Post: AdminFiles (Multipul)
        [HttpPost]
        public ActionResult Index(HttpPostedFileBase[] FileUpload1, string[] fileDesc, int folderID)
        {
            try
            {
                var index = 0;
                foreach (HttpPostedFileBase file in FileUpload1)
                {
                    string fileName = System.IO.Path.GetFileName(file.FileName);
                    string mimeType = file.ContentType;
                    int fileLength = file.ContentLength;

                    Stream fileStream = file.InputStream;
                    byte[] fileData = new byte[fileLength];
                    fileStream.Read(fileData, 0, fileLength);

                    HospiceNiagara.Models.DatabaseModels.File newFile = new HospiceNiagara.Models.DatabaseModels.File
                    {
                        FileContent = fileData,
                        MimeType = mimeType,
                        FileName = fileName,
                        FileDescription = fileDesc[index],
                        FolderID = folderID
                    };

                    db.Files.Add(newFile);
                    index++;
                }
                SaveChanges(db);

                SelectListItem allOption = new SelectListItem() { Value = "0", Text = "All" };
                SelectList folders = new SelectList(db.Folders.OrderBy(x => x.FolderName), "ID", "FolderName");

                var folderList = folders.ToList();
                folderList.Insert(0, allOption);



                ViewBag.FolderID = folderList;

                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.Message = "Error while uploading files";
                return View();
            }
        }

        /*
        // POST: adminFiles
        [HttpPost]
        public ActionResult Index(string fileDesc, int folderID)
        {
            string mimeType = Request.Files[0].ContentType;
            string fileName = Path.GetFileName(Request.Files[0].FileName);
            int fileLength = Request.Files[0].ContentLength;
            if (!(fileName == "" || fileLength == 0))//Looks like we have a file!!!
            {
                Stream fileStream = Request.Files[0].InputStream;
                byte[] fileData = new byte[fileLength];
                fileStream.Read(fileData, 0, fileLength);

               HospiceNiagara.Models.DatabaseModels.File newFile = new HospiceNiagara.Models.DatabaseModels.File
                {
                    FileContent = fileData,
                    MimeType = mimeType,
                    FileName = fileName,
                    FileDescription = fileDesc,
                    FolderID = folderID
                };

                db.Files.Add(newFile);
                SaveChanges(db);
            }

            return RedirectToAction("Index");
        }*/

        private void SaveChanges(DbContext context)
        {
            try
            {
                context.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                StringBuilder sb = new StringBuilder();

                foreach (var failure in ex.EntityValidationErrors)
                {
                    sb.AppendFormat("{0} failed validation\n", failure.Entry.Entity.GetType());
                    foreach (var error in failure.ValidationErrors)
                    {
                        sb.AppendFormat("- {0} : {1}", error.PropertyName, error.ErrorMessage);
                        sb.AppendLine();
                    }
                }

                throw new DbEntityValidationException(
                    "Entity Validation Failed - errors follow:\n" +
                    sb.ToString(), ex
                ); // Add the original exception as the innerException
            }
        }

        // GET: AdminFiles2/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HospiceNiagara.Models.DatabaseModels.File file = db.Files.Find(id);
            if (file == null)
            {
                return HttpNotFound();
            }
            return View(file);
        }

        // GET: AdminFiles2/Create
        public ActionResult Create()
        {
            ViewBag.FolderID = new SelectList(db.Folders, "ID", "FolderName");
            return View();
        }

        // POST: AdminFiles2/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,FileContent,MimeType,FileName,FileDescription,FolderID")] HospiceNiagara.Models.DatabaseModels.File file)
        {
            if (ModelState.IsValid)
            {
                db.Files.Add(file);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.FolderID = new SelectList(db.Folders, "ID", "FolderName", file.FolderID);
            return View(file);
        }

        // GET: AdminFiles2/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HospiceNiagara.Models.DatabaseModels.File file = db.Files.Find(id);
            if (file == null)
            {
                return HttpNotFound();
            }
            ViewBag.FolderID = new SelectList(db.Folders, "ID", "FolderName", file.FolderID);
            return View(file);
        }

        // POST: AdminFiles2/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,FileContent,MimeType,FileName,FileDescription,FolderID")] HospiceNiagara.Models.DatabaseModels.File file)
        {
            if (ModelState.IsValid)
            {
                db.Entry(file).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.FolderID = new SelectList(db.Folders, "ID", "FolderName", file.FolderID);
            return View(file);
        }

        // GET: AdminFiles2/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HospiceNiagara.Models.DatabaseModels.File file = db.Files.Find(id);
            if (file == null)
            {
                return HttpNotFound();
            }
            return View(file);
        }

        // POST: AdminFiles2/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            HospiceNiagara.Models.DatabaseModels.File file = db.Files.Find(id);
            db.Files.Remove(file);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public FileContentResult FileDownload(int id)
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
