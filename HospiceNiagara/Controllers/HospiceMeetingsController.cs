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
    public class HospiceMeetingsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: HospiceMeetings
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
            var meet = from m in db.Meetings
                       select m;

            //Filter
            if (!String.IsNullOrEmpty(searchString))
            {
                meet = meet.Where(m => m.Name.Contains(searchString));
            }

          

            //Switch to See what sorting we are going to do
            switch (sortOrder)
            {
                case "name_desc":
                    meet = meet.OrderByDescending(m => m.Name);
                    break;
                case "Date":
                    meet = meet.OrderBy(m => m.Date);
                    break;
                case "date_desc":
                    meet = meet.OrderByDescending(m => m.Date);
                    break;
                default:
                    meet = meet.OrderBy(m => m.Name);
                    break;
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);

            return View(meet.ToPagedList(pageNumber, pageSize));
        }

        // GET: HospiceMeetings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Meetings meetings = db.Meetings.Find(id);
            if (meetings == null)
            {
                return HttpNotFound();
            }
            return View(meetings);
        }      
    }
}
