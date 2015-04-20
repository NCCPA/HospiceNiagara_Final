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
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Text;
using System.Data.Entity.Validation;
using System.Collections.Generic;

namespace HospiceNiagara.Controllers
{   
    public class AdminScheduleController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: AdminSchedule
        public ActionResult Index()
        {
            

            return View();
        }
               
        [HttpGet]
        public JsonResult GetAnnouncements(double start, double end)
        {
            DateTime startDate = ConvertFromUnixTimestamp(start);
            DateTime endDate = ConvertFromUnixTimestamp(end);

            var announcements = db.Announcements.Where(a => a.Date > startDate && a.Date < endDate).Take(10).ToList();
            List<CalenderViewModel> announcList = new List<CalenderViewModel>();

            foreach (var a in announcements)
            {
                CalenderViewModel newEvent = new CalenderViewModel
                {
                    id = a.ID.ToString(),
                    title = a.Title,
                    start = a.Date.ToString("s"),
                    end = a.Date.AddHours(1).ToString("s"),
                    allDay = false
                };

                announcList.Add(newEvent);
            }

            var rows = announcList.ToArray();
                        
            return Json(rows,JsonRequestBehavior.AllowGet);
        }

        static DateTime ConvertFromUnixTimestamp(double timestamp)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return origin.AddSeconds(timestamp);
        }

    }
}