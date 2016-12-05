using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PowerJump.Models;
using PowerJump.ModelView;
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
            var projects = db.Galleries.OfType<Project>().Include(b => b.ProjectLocales);

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

            Project project = db.Galleries.OfType<Project>().Where(x => x.GalleryId == id).FirstOrDefault();
            ProjectLocales projectLocales = (ProjectLocales)project.ProjectLocales.Where(x => x.Project.GalleryId == id).FirstOrDefault();

            var compositeModel = new ProjectVM();
            compositeModel.ProjectModel = project;
            compositeModel.ProjectLocalesModel = projectLocales;

            if (compositeModel == null)
                return HttpNotFound();

            return View(compositeModel);
        }

        // POST: Admin/Projects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Project project, ProjectVM compositeModel)
        {
            if (ModelState.IsValid)
            {
                project = db.Galleries.OfType<Project>()
                    .Where(x => x.GalleryId == compositeModel.ProjectModel.GalleryId)
                    .Include(x => x.ProjectLocales)
                    .FirstOrDefault();
                project.Date = compositeModel.ProjectModel.Date;
                project.ProjectLocales.Single().Title = compositeModel.ProjectLocalesModel.Title;
                project.ProjectLocales.Single().Description = compositeModel.ProjectLocalesModel.Description;

                db.Entry(project).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(compositeModel);
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
            if (Directory.Exists(uploadDir))
                Directory.Delete(Server.MapPath(uploadDir), true);
            if (project.ProjectLocales.Count() > 0)
            {
                foreach (var locale in project.ProjectLocales.ToList())
                {
                    db.ProjectLocales.Remove(locale);
                }
            }
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
