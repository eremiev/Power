using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PowerJump.Models;

namespace PowerJump.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin,Normal")]
    public class EventsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: admin/events
        public ActionResult Index()
        {
            var events = db.Galleries.OfType<Event>().ToList();
            return View(events);
        }

        // GET: admin/events/details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = (Event)db.Galleries.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }

        // GET: admin/events/create
        public ActionResult Create()
        {
            return View();
        }

        // POST: admin/events/create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "GalleryId,Title,Description,Date")] Event @event)
        {
            if (ModelState.IsValid)
            {
                db.Galleries.Add(@event);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(@event);
        }

        // GET: admin/events/edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = (Event)db.Galleries.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }

        // POST: admin/events/edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "GalleryId,Title,Description,Date")] Event @event)
        {
            if (ModelState.IsValid)
            {
                db.Entry(@event).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(@event);
        }

        // GET: admin/events/delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = (Event)db.Galleries.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }

        // POST: admin/events/delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Event @event = (Event)db.Galleries.Find(id);
            db.Galleries.Remove(@event);
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
