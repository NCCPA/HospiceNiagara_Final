﻿using System;
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
    [Authorize(Roles = "Admin")]
    public class AdminMeetingsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: AdminMeetings
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            //Set Sort Order
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";


            if(searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            //Grab all Meetings
            var meetings = from m in db.Meetings
                           select m;

            //Filter
            if (!String.IsNullOrEmpty(searchString))
            {
                meetings = meetings.Where(m => m.Name.Contains(searchString));
            }

            //Switch to See what sorting we are going to do
            switch (sortOrder)
            {
                case "name_desc":
                    meetings = meetings.OrderByDescending(m => m.Name);
                    break;
                case "Date":
                    meetings = meetings.OrderBy(m => m.Date);
                    break;
                case "date_desc":
                    meetings = meetings.OrderByDescending(m => m.Date);
                    break;
                default:
                    meetings = meetings.OrderBy(m => m.Name);
                    break;
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);

            return View(meetings.ToPagedList(pageNumber,pageSize));
        }

        // GET: AdminMeetings/Details/5
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

        // GET: AdminMeetings/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdminMeetings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Type,Name,Description,Date,Length,Location,Requirements,isVisible,StartTime,EndTime,StaffLeadID,CreatedByID")] Meetings meetings)
        {
            if (ModelState.IsValid)
            {
                db.Meetings.Add(meetings);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(meetings);
        }

        // GET: AdminMeetings/Edit/5
        public ActionResult Edit(int? id)
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

        // POST: AdminMeetings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Type,Name,Description,Date,Length,Location,Requirements,isVisible,StartTime,EndTime,StaffLeadID,CreatedByID")] Meetings meetings)
        {
            if (ModelState.IsValid)
            {
                db.Entry(meetings).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(meetings);
        }

        // GET: AdminMeetings/Delete/5
        public ActionResult Delete(int? id)
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

        // POST: AdminMeetings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Meetings meetings = db.Meetings.Find(id);
            db.Meetings.Remove(meetings);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
