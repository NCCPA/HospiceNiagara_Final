using HospiceNiagara.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace HospiceNiagara.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        HelperClass helperClass = new HelperClass();
        private ApplicationDbContext db = new ApplicationDbContext();


        // GET: Profile
        public ActionResult Index()
        {
            //Get currently logged in Person
            var userID = User.Identity.GetUserId();

            var userInfo = db.Users.Where(x => x.Id == userID).SingleOrDefault();

            return View(userInfo);
        }

        // GET: Edit
        [HttpGet]
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var memberViewModel = db.Users.Where(x => x.Id == id).SingleOrDefault();

            return View(memberViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,FirstName,LastName,Email,PhoneNumber,PhoneExt,Bio")] ApplicationUser model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            //if it bring back 1 user with that email then
            var selectedUser = db.Users.Where(m => m.Email == model.Email);

            //user null because email is not taken update user
            if ( selectedUser.Count() == 0 || helperClass.CurrentEmail(model.Id,model.Email) )
            {
                //get information
                var userFound = db.Users.Where(m => m.Id == model.Id).SingleOrDefault();

                //Update the feilds they are allowed to edit
                userFound.FirstName = model.FirstName;
                userFound.LastName = model.LastName;
                userFound.UserName = model.Email;
                userFound.Email = model.Email;
                userFound.PhoneNumber = model.PhoneNumber;
                userFound.PhoneExt = model.PhoneExt;
                userFound.Bio = model.Bio;

                //save the changes that occured
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            else
            {
                //there is another email in the database with that name already add error to model
                ModelState.AddModelError("Email", "E-Mail already registered");
                return View(model);
            }

        }


        /// <summary>
        /// Show the Profile Picture in the Left Nav-bar
        /// </summary>
        /// <param name="id"></param>
        public void ShowPic(string id)
        {
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));


            //If Id is null then not coming from admin page
            if (id != "")
            {
                //Get Current Id and Grab Profile Picture
                var currentUserId = User.Identity.GetUserId();
                var curUser = manager.FindById(currentUserId);
                Response.Buffer = true;
                Response.Clear();
                Response.ContentType = curUser.MimeType;
                Response.BinaryWrite(curUser.ProfilePicture);
                Response.End();
            }
        }
    }
}
