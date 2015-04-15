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
    public class AdminSubRolesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private HelperClass helperClass = new HelperClass();

        // GET: AdminSubRoles
        [HttpGet]
        public ActionResult Index()
        {
            //Get all roles from db
            SelectList roles = new SelectList(db.Roles.OrderBy(x => x.Name).Where(x => x.Name != "SuperAdmin"), "ID", "Name");
            var roleList = roles.ToList();
            
            //add to rolelist select a role option
            SelectListItem allOption = new SelectListItem() { Value = "0", Text = "Select A Role" };
            roleList.Insert(0, allOption);
            ViewBag.RolesList = roleList;            

           
            

            return View(db.SubRoles.Where(x=>x.Name == "SelectARole").ToList());
        }

        // POST: AdminSubRoles
        [HttpPost]
        public ActionResult Index(string roleID)
        {
            //Take care of drop down list
            SelectList roles = new SelectList(db.Roles.OrderBy(x => x.Name).Where(x => x.Name != "SuperAdmin"), "ID", "Name", roleID);
            var roleList = roles.ToList();
            ViewBag.RolesList = roleList;

            //get model with those specific roleID
            var foundRoles = db.SubRoles.Where(x => x.RoleID == roleID);

            return View(foundRoles.ToList());
        }


        // GET: AdminSubRoles/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubRoles subRoles = db.SubRoles.Find(id);
            if (subRoles == null)
            {
                return HttpNotFound();
            }
            return View(subRoles);
        }

        // GET: AdminSubRoles/Create
        public ActionResult Create()
        {
            //Get all roles from db
            SelectList roles = new SelectList(db.Roles.OrderBy(x => x.Name).Where(x => x.Name != "SuperAdmin"), "ID", "Name");
            var roleList = roles.ToList();
            ViewBag.RolesList = roleList;
            return View();
        }

        // POST: AdminSubRoles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,RoleID")] SubRoles subRoles)
        {
            var roleList = helperClass.getRolesList();
            ViewBag.RolesList = roleList;


            if (ModelState.IsValid)
            {
                db.SubRoles.Add(subRoles);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(subRoles);
        }

        // GET: AdminSubRoles/Edit/5
        public ActionResult Edit(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubRoles subRoles = db.SubRoles.Find(id);



            SelectList roles = new SelectList(db.Roles.OrderBy(x => x.Name).Where(x => x.Name != "SuperAdmin"), "ID", "Name", subRoles.RoleID);
            ViewBag.RolesList = roles.ToList();         
           
            if (subRoles == null)
            {
                return HttpNotFound();
            }
            return View(subRoles);
        }

        // POST: AdminSubRoles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,RoleID")] SubRoles subRoles)
        {
            if (ModelState.IsValid)
            {
                db.Entry(subRoles).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(subRoles);
        }

        // GET: AdminSubRoles/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubRoles subRoles = db.SubRoles.Find(id);
            if (subRoles == null)
            {
                return HttpNotFound();
            }
            return View(subRoles);
        }

        // POST: AdminSubRoles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SubRoles subRoles = db.SubRoles.Find(id);
            db.SubRoles.Remove(subRoles);
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
