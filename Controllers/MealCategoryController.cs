using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IdentitySample.Models;
using TheHappyMeal.Models;

namespace TheHappyMeal.Controllers
{
    [Authorize(Roles = "Admin")]
    public class MealCategoryController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: MealCategory
        public ActionResult Index()
        {
            return View(db.Categories.ToList());
        }

        // GET: MealCategory/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MealCategory mealCategory = db.Categories.Find(id);
            if (mealCategory == null)
            {
                return HttpNotFound();
            }
            return View(mealCategory);
        }

        // GET: MealCategory/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MealCategory/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,CoverImage")] MealCategory mealCategory, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                string path = Path.Combine(Server.MapPath("~/Uploads/Images"), upload.FileName);
                upload.SaveAs(path);
                mealCategory.CoverImage = upload.FileName;
                db.Categories.Add(mealCategory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(mealCategory);
        }

        // GET: MealCategory/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MealCategory mealCategory = db.Categories.Find(id);
            if (mealCategory == null)
            {
                return HttpNotFound();
            }
            return View(mealCategory);
        }

        // POST: MealCategory/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,CoverImage")] MealCategory mealCategory, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                string oldPath = Path.Combine(Server.MapPath("~/Uploads/Images"), mealCategory.CoverImage);

                if (upload != null)
                {
                    System.IO.File.Delete(oldPath);
                    string path = Path.Combine(Server.MapPath("~/Uploads/Images"), upload.FileName);
                    upload.SaveAs(path);
                    mealCategory.CoverImage = upload.FileName;
                }

                db.Entry(mealCategory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(mealCategory);
        }

        // GET: MealCategory/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MealCategory mealCategory = db.Categories.Find(id);
            if (mealCategory == null)
            {
                return HttpNotFound();
            }
            return View(mealCategory);
        }

        // POST: MealCategory/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MealCategory mealCategory = db.Categories.Find(id);
            string imageDel = Path.Combine(Server.MapPath("~/Uploads/Images"), db.Categories.Single(m => m.Id == id).CoverImage);
            FileInfo fi = new FileInfo(imageDel);
            if (fi != null)
            {
                System.IO.File.Delete(imageDel);
                fi.Delete();
            }

            db.Categories.Remove(mealCategory);
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
