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
    public class HospiceAnnouncementsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: HospiceAnnouncements
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
            var announce = from m in db.Announcements
                       select m;

            //Filter
            if (!String.IsNullOrEmpty(searchString))
            {
                announce = announce.Where(m => m.Title.Contains(searchString));
            }



            //Switch to See what sorting we are going to do
            switch (sortOrder)
            {
                case "name_desc":
                    announce = announce.OrderByDescending(m => m.Title);
                    break;
                case "Date":
                    announce = announce.OrderBy(m => m.Date);
                    break;
                case "date_desc":
                    announce = announce.OrderByDescending(m => m.Date);
                    break;
                default:
                    announce = announce.OrderBy(m => m.Title);
                    break;
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);

            return View(announce.ToPagedList(pageNumber, pageSize));
        }

        // GET: HospiceAnnouncements/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Announcement announcement = db.Announcements.Find(id);
            if (announcement == null)
            {
                return HttpNotFound();
            }
            return View(announcement);
        }

        // GET: HospiceAnnouncements/Create
        public ActionResult Create()
        {
            return View();
        }
    }
}
