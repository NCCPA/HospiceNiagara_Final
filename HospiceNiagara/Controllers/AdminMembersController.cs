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
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Text;
using System.Data.Entity.Validation;

namespace HospiceNiagara.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminMembersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private HelperClass helperClass = new HelperClass();

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

            ViewBag.roleName = helperClass.roleNameByUserId(id);

            if (memberViewModel == null)
            {
                return HttpNotFound();
            }
            return View(memberViewModel);
        }

        ////POST: AdminMembers/POSTMAINROLES
        //public JsonResult PostMainRoles()
        //{
        //    //Open Database
        //    ApplicationDbContext db = new ApplicationDbContext();

        //    //grab roles
        //    //var mainRoles 
        //    return Json(mainRoles);
        //}

        // GET: AdminMembers/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }            

            ApplicationUser memberViewModel = db.Users.Find(id);
                       
            SelectList roles = new SelectList(db.Roles.OrderBy(x => x.Name), "ID", "Name");
            ViewBag.RolesList = roles;

            var member = db.Users.Include(f => f.Roles);
            
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
        public ActionResult Edit([Bind(Include = "id,FirstName,LastName,Email,PhoneNumber,PhoneExt,IsContact,isActive,Position,PositionDescription,Bio")] ApplicationUser memberViewModel, string roleID)
        {
            //get roles and add to list
            ViewBag.RolesList = helperClass.getRolesList(memberViewModel.Id);
            
            if (!ModelState.IsValid)
            {
                return View(memberViewModel);
            }

            //find all users with same email, should only be 1 if it exists
            var selectedUser = db.Users.Where(m => m.Email == memberViewModel.Email);

            //check email, if 0 users have this email OR this users current owns this email allow him to update.
            if ( selectedUser.Count() == 0 ||  helperClass.CurrentEmail(memberViewModel.Id, memberViewModel.Email))
            {
                //grab single entity
                var editUser = selectedUser.FirstOrDefault();

                //edit the fields that apply to them
                editUser.FirstName = memberViewModel.FirstName;
                editUser.LastName = memberViewModel.LastName;
                editUser.Email = memberViewModel.Email;
                editUser.UserName = memberViewModel.Email; //username same as email change it
                editUser.PhoneExt = memberViewModel.PhoneNumber;
                editUser.PhoneExt = memberViewModel.PhoneExt;
                editUser.IsContact = memberViewModel.IsContact;
                editUser.isActive = memberViewModel.isActive;
                editUser.Position = memberViewModel.Position;
                editUser.PositionDescription = memberViewModel.PositionDescription;
                editUser.Bio = memberViewModel.Bio;
                
                //save the changed made
                SaveChanges(db);

                //Removes user from old role, adds to new role
                helperClass.ChangeRoles(memberViewModel.Id, roleID);

                return RedirectToAction("Index");
            }
            else
            {
                //there is another email in the database with that name already add error to model
                ModelState.AddModelError("Email", "E-Mail already registered");
                return View(memberViewModel);                        

            }                                       
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

        // Method For Finding InnerException
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
    }
}
