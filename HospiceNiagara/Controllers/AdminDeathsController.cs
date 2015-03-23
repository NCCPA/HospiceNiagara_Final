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

namespace HospiceNiagara.Controllers
{
    public class AdminDeathsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: AdminDeaths
        public ActionResult Index()
        {
            return View(db.Deaths.ToList());
        }

        // GET: AdminDeaths/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Deaths deaths = db.Deaths.Find(id);
            if (deaths == null)
            {
                return HttpNotFound();
            }
            return View(deaths);
        }

        // GET: AdminDeaths/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdminDeaths/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,Date,Location,Note,Visible,CreatedByID")] Deaths deaths)
        {
            if (ModelState.IsValid)
            {
                db.Deaths.Add(deaths);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(deaths);
        }

        // GET: AdminDeaths/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Deaths deaths = db.Deaths.Find(id);
            if (deaths == null)
            {
                return HttpNotFound();
            }
            return View(deaths);
        }

        // POST: AdminDeaths/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Date,Location,Note,Visible,CreatedByID")] Deaths deaths)
        {
            if (ModelState.IsValid)
            {
                db.Entry(deaths).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(deaths);
        }

        // GET: AdminDeaths/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Deaths deaths = db.Deaths.Find(id);
            if (deaths == null)
            {
                return HttpNotFound();
            }
            return View(deaths);
        }

        // POST: AdminDeaths/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Deaths deaths = db.Deaths.Find(id);
            db.Deaths.Remove(deaths);
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
