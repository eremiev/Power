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
        public ActionResult Create([Bind(Include = "Events,Projects")] Photo photoModel, FormCollection form, HttpPostedFileBase uploadImage)
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

            if (!string.IsNullOrEmpty(dropdownEvent))
                gallery = db.Galleries.Find(Convert.ToInt32(dropdownEvent));
            else if (!string.IsNullOrEmpty(dropdownProject))
                gallery = db.Galleries.Find(Convert.ToInt32(dropdownProject));
            else
                ModelState.AddModelError("dropdown", "Please chose Event or Project!");


            if (uploadImage == null || uploadImage.ContentLength == 0)
                ModelState.AddModelError("uploadImage", "This field is required");
            else if (!validImageTypes.Contains(uploadImage.ContentType))
                ModelState.AddModelError("uploadImage", "Please choose either a GIF, JPEG or PNG image.");

            if (ModelState.IsValid)
            {
                if (gallery != null)
                {
                    if (uploadImage != null && uploadImage.ContentLength > 0)
                    {

                        Photo photo = new Photo();
                        photo.GalleryId = gallery.GalleryId;
                        photo = db.Photos.Add(photo);
                        db.SaveChanges();

                        string date = DateTime.Now.ToString("M_d_yyyy");
                        string imageName = photo.Id + "_" + date + "_" + uploadImage.FileName;
                        var uploadDir = "~/Content/uploads/" + gallery.GalleryId + "/";
                        var imagePath = Path.Combine(Server.MapPath(uploadDir), imageName);
                        var imageUrl = Path.Combine(uploadDir, imageName);
                        if (!Directory.Exists(uploadDir))
                            Directory.CreateDirectory(Server.MapPath(uploadDir));
                        uploadImage.SaveAs(imagePath);

                        photo.Path = imageUrl;
                        db.SaveChanges();
                    }
                }

                return RedirectToAction("Index");
            }

            ViewBag.Events = new SelectList(db.Galleries.OfType<Event>(), "GalleryId", "Title");
            ViewBag.Projects = new SelectList(db.Galleries.OfType<Project>(), "GalleryId", "Title");

            return View(photoModel);
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
        public ActionResult DeleteConfirmed(int id, string goToController, string goToAction)
        {
            Photo photo = db.Photos.Find(id);

            System.IO.File.Delete(Server.MapPath(photo.Path));
            db.Photos.Remove(photo);
            db.SaveChanges();
            return RedirectToAction(goToAction, goToController);
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

