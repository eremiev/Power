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
            //   var projects = db.Galleries.ToList();
            var projects = db.Galleries.OfType<Project>().ToList();
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
        public ActionResult Create([Bind(Include = "Title,Description,Date")] Project project, HttpPostedFileBase photo)
        {
            var validImageTypes = new string[]
             {
                    "image/gif",
                    "image/png",
                    "image/jpeg"
             };

            if (photo == null || photo.ContentLength == 0)
            {
                ModelState.AddModelError("photo", "This field is required");
            }
            else if (!validImageTypes.Contains(photo.ContentType))
            {
                ModelState.AddModelError("photo", "Please choose either a GIF, JPEG or PNG image.");
            }

            if (ModelState.IsValid)
            {
                var proj = db.Galleries.Add(project);
                db.SaveChanges();

                if (photo != null && photo.ContentLength > 0)
                {
                    string name = Path.GetFileName(photo.FileName);
                    var uploadDir = "~/Content/uploads";
                    var imagePath = Path.Combine(Server.MapPath(uploadDir), photo.FileName);
                    var imageUrl = Path.Combine(uploadDir, photo.FileName);

                    photo.SaveAs(imagePath);

                    Photo foto = new Photo();
                    foto.Path = imageUrl;
                    foto.GalleryId = proj.GalleryId;
                    db.Photos.Add(foto);
                    db.SaveChanges();
                }

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
        public ActionResult Edit([Bind(Include = "GalleryId,Title,Description,Date")] Project project)
        {
            if (ModelState.IsValid)
            {
                db.Entry(project).State = EntityState.Modified;
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
            // TODO: delete all photos releated to this project
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
