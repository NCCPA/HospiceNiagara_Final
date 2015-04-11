using HospiceNiagara.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace HospiceNiagara.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        public HelperClass helperClass = new HelperClass();
        private ApplicationDbContext db = new ApplicationDbContext();


        // GET: Profile
        [HttpGet]
        public ActionResult Index()
        {
            //Get currently logged in Person
            var userID = User.Identity.GetUserId();

            var userInfo = db.Users.Where(x => x.Id == userID).SingleOrDefault();

            return View(userInfo);
        }
        
        //POST: Picture Upload
        [HttpPost]
        public ActionResult UploadPicture()
        {
            //Get Current User Depending on Profile Location - Admin or Owner
            var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
            var manager = new UserManager<ApplicationUser>(store);

            var currentUser = manager.FindById(User.Identity.GetUserId());

            string mimeType = Request.Files[0].ContentType;
            string fileName = Path.GetFileName(Request.Files[0].FileName);
            int fileLength = Request.Files[0].ContentLength;

            if (!(fileName == "" || fileLength == 0))//Looks like we have a file!!!
            {
                Stream fileStream = Request.Files[0].InputStream;
                byte[] fileData = new byte[fileLength];
                fileStream.Read(fileData, 0, fileLength);

                //Add File To user
                currentUser.ProfilePicture = fileData;
                currentUser.MimeType = mimeType;

                //Update User and Save the current Changes
                store.Context.SaveChanges();                
                
                return RedirectToAction("Index");
            }

            return View(currentUser);
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

        // POST: Edit
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
            if (selectedUser.Count() == 0 || helperClass.CurrentEmail(model.Id, model.Email))
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


            //If Id is null then continue.
            if (id != "")
            {
                //Get Current Id and Grab Profile Picture
                var currentUserId = User.Identity.GetUserId();
                var curUser = manager.FindById(currentUserId);

                if (curUser != null)
                {
                    if (curUser.ProfilePicture != null)
                    {
                        Response.Buffer = true;
                        Response.Clear();
                        Response.ContentType = curUser.MimeType;
                        Response.BinaryWrite(curUser.ProfilePicture);
                        Response.End();
                    }
                }
            }
        }
    }
}
