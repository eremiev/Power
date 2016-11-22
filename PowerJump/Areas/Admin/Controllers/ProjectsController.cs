using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PowerJump.Models;
using System.IO;

namespace PowerJump.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin,Normal")]
    public class ProjectsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin/Projects
        public ActionResult Index()
        {
            var projects = db.Galleries.OfType<Project>().Include(b => b.ProjectLocales).ToList();

            return View(projects);
        }

        // GET: Admin/Projects/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = (Project)db.Galleries.Find(id);
            
                //OfType<Project>().Include(b => b.ProjectLocales).Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // GET: Admin/Projects/Create
        public ActionResult Create()
        {

            return View();
        }

        // POST: Admin/Projects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Prefix = "Item1", Include = "Date")] Project project, [Bind(Prefix = "Item2", Include = "Title,Description")] ProjectLocales locale)
        {
            if (ModelState.IsValid)
            {
                locale.Project = project;
                locale.Locale = "en";
                db.ProjectLocales.Add(locale);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(project);
        }

        // GET: Admin/Projects/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Project project = (Project)db.Galleries.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Admin/Projects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Prefix = "Item1", Include = "Date")] Project project, [Bind(Prefix = "Item2", Include = "Title,Description")] ProjectLocales locale)
        {
            //project, locale = null
            if (ModelState.IsValid)
            {
                db.Entry(project).State = EntityState.Modified;
                db.Entry(locale).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(project);
        }

        // GET: Admin/Projects/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = (Project)db.Galleries.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Admin/Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Project project = (Project)db.Galleries.Find(id);
            var uploadDir = "~/Content/uploads/" + project.GalleryId;
            Directory.Delete(Server.MapPath(uploadDir), true);
            db.Galleries.Remove(project);
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
