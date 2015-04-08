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
using PagedList;

namespace HospiceNiagara.Controllers
{
    public class HospiceDeathsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: HospiceDeaths
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            SelectListItem allOption = new SelectListItem() { Value = "0", Text = "All" };

            //Set Sort Order
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";


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
            var death = from m in db.Deaths
                       select m;

            //Filter
            if (!String.IsNullOrEmpty(searchString))
            {
                death = death.Where(m => m.Name.Contains(searchString));
            }



            //Switch to See what sorting we are going to do
            switch (sortOrder)
            {
                case "name_desc":
                    death = death.OrderByDescending(m => m.Name);
                    break;
                case "Date":
                    death = death.OrderBy(m => m.Date);
                    break;
                case "date_desc":
                    death = death.OrderByDescending(m => m.Date);
                    break;
                default:
                    death = death.OrderBy(m => m.Name);
                    break;
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);

            return View(death.ToPagedList(pageNumber, pageSize));
        }

        // GET: HospiceDeaths/Details/5
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
    }
}
