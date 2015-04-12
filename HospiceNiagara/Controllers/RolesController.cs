using HospiceNiagara.Models;
using HospiceNiagara.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HospiceNiagara.Controllers
{
    public class RolesController : Controller
    {

        private ApplicationDbContext db = new ApplicationDbContext();


        // GET: Roles
        public ActionResult Index()
        {
            var roleList = db.Roles.Where(x => x.Name != "SuperAdmin"); // not allowed to touch SuperAdmin since it controlls everything
            return View(roleList.ToList());
        }


        //GET: Create
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        //POST: Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RolesViewModel model)
        {
            if (ModelState.IsValid)
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole
                {
                    Name = model.Name                                      
                };

                //Add & Save
                db.Roles.Add(role);
                db.SaveChanges();

                return RedirectToAction("Index", "Roles");
            }

            return View(model);
        }

       


    }
}