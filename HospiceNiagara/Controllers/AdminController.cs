using HospiceNiagara.Models;
using HospiceNiagara.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace HospiceNiagara.Controllers
{
    public class AdminController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }


        //GET: Members
        [HttpGet]
        public ActionResult Members_List(string searchString)
        {
            var viewModel = from m in db.Users
                            select new MemberViewModel
                            {
                                id = m.Id,
                                FirstName = m.FirstName,
                                LastName = m.LastName,
                                PhoneNumber = m.PhoneNumber,
                                PhoneExt = m.PhoneExt,
                                Email = m.Email,
                                IsContact = m.IsContact
                            };

            if (!String.IsNullOrEmpty(searchString))
            {
                viewModel = viewModel.Where(s => s.FirstName.Contains(searchString));
            }

            return View(viewModel.ToList());
        }

        //Get View: Members
        [HttpGet]
        public ActionResult Members_View(string id)
        {
            //Check to make sure there is an ID sent
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ApplicationUser user = db.Users.Find(id);

            return View(user);
        }

        //Get Edit: Members
        [HttpGet]
        public ActionResult Members_Edit(string id)
        {
            //Check to make sure there is an ID sent
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ApplicationUser user = db.Users.Find(id);

            return View(user);
        }

        //Post Edit: Members
        [HttpPost]
        public ActionResult Members_Edit([Bind(Include = "id, UserName, FirstName,LastName,Email,PhoneNumber,PhoneExt,isContact, Position, PositionDescription, Bio")] ApplicationUser user)
        {
            if(ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                SaveChanges(db);
                return RedirectToAction("Members_List");
            }
            return View(user);
            
        }

        //GET Delete: Member        
        [HttpGet]
        public ActionResult Members_Delete(string id)
        {            
            ApplicationUser user = db.Users.Find(id);            
            return View(user);
        }

        //Post Delete: Member
        // POST: /Movies/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Members_Delete_Post(string id)
        {
            ApplicationUser user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Members_List");
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

    }
}