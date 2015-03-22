using HospiceNiagara.Models;
using HospiceNiagara.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
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
                                FirstName = m.FirstName,
                                LastName = m.LastName,
                                PhoneNumber = m.PhoneNumber,
                                PhoneExt = m.PhoneExt,
                                Email = m.Email
                            };

            if (!String.IsNullOrEmpty(searchString))
            {
                viewModel = viewModel.Where(s => s.FirstName.Contains(searchString));
            }

            return View(viewModel.ToList());
        }

    }
}