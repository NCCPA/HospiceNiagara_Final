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

namespace HospiceNiagara.Controllers
{
    public class AdminResourcesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: AdminFiles
        public ActionResult Index()
        {
            ViewBag.FolderID = new SelectList(db.Folders, "ID", "FolderName");
            var files = db.Files.Include(f => f.Folder);
            return View(db.Files.ToList());
        }

        // POST: adminFiles
        [HttpPost]
        public ActionResult Index(string name, string fileDesc)
        {
            string mimeType = Request.Files[0].ContentType;            
            int fileLength = Request.Files[0].ContentLength;
            if (!(name == "" || fileLength == 0))//Looks like we have a file!!!
            {
                Stream fileStream = Request.Files[0].InputStream;
                byte[] fileData = new byte[fileLength];
                fileStream.Read(fileData, 0, fileLength);

                HospiceNiagara.Models.DatabaseModels.File newFile = new HospiceNiagara.Models.DatabaseModels.File
                {
                    FileContent = fileData,
                    MimeType = mimeType,
                    FileName = name,
                    FileDescription = fileDesc
                };

                db.Files.Add(newFile);
                SaveChanges(db);
            }

            return RedirectToAction("Index");
        }

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

        // GET: AdminFiles/Details/5
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

        // GET: AdminFiles/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdminFiles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,FileContent,MimeType,FileName,FileDescription")] HospiceNiagara.Models.DatabaseModels.File file)
        {
            if (ModelState.IsValid)
            {
                db.Files.Add(file);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(file);
        }

        // GET: AdminFiles/Edit/5
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
            return View(file);
        }

        // POST: AdminFiles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,FileContent,MimeType,FileName,FileDescription")] HospiceNiagara.Models.DatabaseModels.File file)
        {
            if (ModelState.IsValid)
            {
                db.Entry(file).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(file);
        }

        // GET: AdminFiles/Delete/5
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

        // POST: AdminFiles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            HospiceNiagara.Models.DatabaseModels.File file = db.Files.Find(id);
            db.Files.Remove(file);
            db.SaveChanges();
            return RedirectToAction("Index");
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
