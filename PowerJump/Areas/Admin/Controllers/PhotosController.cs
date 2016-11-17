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
    [Authorize(Roles = "Admin")]
    public class PhotosController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin/Photos
        public ActionResult Index()
        {
            ViewBag.Events = db.Galleries.OfType<Event>().ToList();
            ViewBag.Projects = db.Galleries.OfType<Project>().ToList();

            return View();
        }

        // GET: Admin/Photos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var gallery = db.Photos.Where(x => x.GalleryId == id).ToList();
            if (gallery == null)
                return HttpNotFound();

            return View(gallery);
        }

        // GET: Admin/Photos/Create
        public ActionResult Create()
        {
            ViewBag.Events = new SelectList(db.Galleries.OfType<Event>(), "GalleryId", "Title");
            ViewBag.Projects = new SelectList(db.Galleries.OfType<Project>(), "GalleryId", "Title");
            return View();
        }

        // POST: Admin/Photos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Events,Projects")] Photo photo, FormCollection form, HttpPostedFileBase image)
        {
            Gallery gallery = null;
            string dropdownEvent = form["Events"];
            string dropdownProject = form["Projects"];

            var validImageTypes = new string[]
         {
                    "image/gif",
                    "image/png",
                    "image/jpeg"
         };
            //TODO: save image with same filename
            if (!string.IsNullOrEmpty(dropdownEvent))
                gallery = db.Galleries.Find(Convert.ToInt32(dropdownEvent));
            else if (!string.IsNullOrEmpty(dropdownProject))
                gallery = db.Galleries.Find(Convert.ToInt32(dropdownProject));
            else
                ModelState.AddModelError("dropdown", "Please chose Event or Project!");


            if (image == null || image.ContentLength == 0)
                ModelState.AddModelError("image", "This field is required");
            else if (!validImageTypes.Contains(image.ContentType))
                ModelState.AddModelError("image", "Please choose either a GIF, JPEG or PNG image.");

            if (ModelState.IsValid)
            {
                if (gallery != null)
                {
                    if (image != null && image.ContentLength > 0)
                    {
                        string name = Path.GetFileName(image.FileName);
                        var uploadDir = "~/Content/uploads";
                        var imagePath = Path.Combine(Server.MapPath(uploadDir), image.FileName);
                        var imageUrl = Path.Combine(uploadDir, image.FileName);
                        image.SaveAs(imagePath);

                        Photo foto = new Photo();
                        foto.Path = imageUrl;
                        foto.GalleryId = gallery.GalleryId;
                        db.Photos.Add(foto);
                        db.SaveChanges();
                    }
                }

                return RedirectToAction("Index");
            }

            ViewBag.Events = new SelectList(db.Galleries.OfType<Event>(), "GalleryId", "Title");
            ViewBag.Projects = new SelectList(db.Galleries.OfType<Project>(), "GalleryId", "Title");

            return View(photo);
        }

        // GET: Admin/Photos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Photo photo = db.Photos.Find(id);
            if (photo == null)
            {
                return HttpNotFound();
            }
            ViewBag.GalleryId = new SelectList(db.Galleries, "GalleryId", "GalleryId", photo.GalleryId);
            return View(photo);
        }

        // POST: Admin/Photos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Path,GalleryId")] Photo photo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(photo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.GalleryId = new SelectList(db.Galleries, "GalleryId", "GalleryId", photo.GalleryId);
            return View(photo);
        }

        // GET: Admin/Photos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Photo photo = db.Photos.Find(id);
            if (photo == null)
            {
                return HttpNotFound();
            }
            return View(photo);
        }

        // POST: Admin/Photos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, string goTo)
        {
            Photo photo = db.Photos.Find(id);

            System.IO.File.Delete(Server.MapPath(photo.Path));
            db.Photos.Remove(photo);
            db.SaveChanges();
            return RedirectToAction("Index", goTo);
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

