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

namespace HospiceNiagara.Controllers
{
    public class HospiceDeathsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: HospiceDeaths
        public ActionResult Index()
        {
            return View(db.Deaths.ToList());
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
