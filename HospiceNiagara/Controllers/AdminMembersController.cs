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
using PagedList;

namespace HospiceNiagara.Controllers
{
    public class AdminMembersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: AdminMembers
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {

            //Set Sort Order
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "LastName" ? "lname_desc" : "LastName";


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
            var users = from m in db.Users
                           select m;

            //Filter
            if (!String.IsNullOrEmpty(searchString))
            {
                users = users.Where(m => m.FirstName.Contains(searchString) || m.LastName.Contains(searchString));
            }

            //Switch to See what sorting we are going to do
            switch (sortOrder)
            {
                case "name_desc":
                    users = users.OrderByDescending(m => m.FirstName);
                    break;
                case "LastName":
                    users = users.OrderBy(m => m.LastName);
                    break;
                case "lname_desc":
                    users = users.OrderByDescending(m => m.LastName);
                    break;
                default:
                    users = users.OrderBy(m => m.FirstName);
                    break;
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);

            return View(users.ToPagedList(pageNumber, pageSize));
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
