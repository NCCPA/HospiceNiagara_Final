using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HospiceNiagara.Models;
using HospiceNiagara.Models.ViewModels;

namespace HospiceNiagara.Controllers
{
    public class AdminMembersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: AdminMembers
        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }

        // GET: AdminMembers/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser memberViewModel = db.Users.Find(id);
            if (memberViewModel == null)
            {
                return HttpNotFound();
            }
            return View(memberViewModel);
        }

        // GET: AdminMembers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdminMembers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MemberViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    PhoneNumber = model.PhoneNumber,
                    PhoneExt = model.PhoneExt,
                    IsContact = model.IsContact,
                    Position = model.Position,
                    PositionDescription = model.PositionDescription,
                    Bio = model.Bio
                };

                //Add & Save
                db.Users.Add(user);
                db.SaveChanges();

                return RedirectToAction("Index", "AdminMembers");
            }

            return View(model);
        }

        // GET: AdminMembers/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser memberViewModel = db.Users.Find(id);
            if (memberViewModel == null)
            {
                return HttpNotFound();
            }
            return View(memberViewModel);
        }

        // POST: AdminMembers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,FirstName,LastName,Email,PhoneNumber,PhoneExt,IsContact,Position,PositionDescription,Bio")] ApplicationUser memberViewModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(memberViewModel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(memberViewModel);
        }

        // GET: AdminMembers/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser memberViewModel = db.Users.Find(id);
            if (memberViewModel == null)
            {
                return HttpNotFound();
            }
            return View(memberViewModel);
        }

        // POST: AdminMembers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            ApplicationUser memberViewModel = db.Users.Find(id);
            db.Users.Remove(memberViewModel);
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
