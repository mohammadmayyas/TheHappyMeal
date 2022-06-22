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
    public class MealController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Meal
        public ActionResult Index()
        {
            var meals = db.Meals.Include(m => m.Category);
            return View(meals.ToList());
        }

        public ActionResult Menu()
        {
            return View();
        }

        // GET: Meal/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Meal meal = db.Meals.Find(id);
            if (meal == null)
            {
                return HttpNotFound();
            }
            return View(meal);
        }

        // GET: Meal/Create
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name");
            ViewBag.CuisineId = new SelectList(db.Cuisines, "CuisineId", "Name");
            ViewBag.Items = new SelectList(db.Items, "Item_Id", "Name");
            return View();
        }

        // POST: Meal/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,Profit,Price,Image,CategoryId,CuisineId,Ingredients,PreparationTime")] Meal meal, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                string path = Path.Combine(Server.MapPath("~/Uploads/Meals"), upload.FileName);
                upload.SaveAs(path);
                meal.Image = upload.FileName;
                db.Meals.Add(meal);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", meal.CategoryId);
            ViewBag.CuisineId = new SelectList(db.Cuisines, "CuisineId", "Name", meal.CuisineId);
            ViewBag.Items = new SelectList(db.Items, "Item_Id", "Name", meal.Ingredients);
            return View(meal);
        }

        // GET: Meal/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Meal meal = db.Meals.Find(id);
            if (meal == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", meal.CategoryId);
            ViewBag.CuisineId = new SelectList(db.Cuisines, "CuisineId", "Name", meal.CuisineId);
            //ViewBag.Items = new SelectList(db.Items, "Item_Id", "Name");
            return View(meal);
        }

        // POST: Meal/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( Meal meal, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                string oldPath = Path.Combine(Server.MapPath("~/Uploads/Meals"), meal.Image);

                if (upload != null)
                {
                    System.IO.File.Delete(oldPath);
                    string path = Path.Combine(Server.MapPath("~/Uploads/Meals"), upload.FileName);
                    upload.SaveAs(path);
                    meal.Image = upload.FileName;
                }

                
                db.Entry(meal).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", meal.CategoryId);
            ViewBag.CuisineId = new SelectList(db.Cuisines, "CuisineId", "Name", meal.CuisineId);
            return View(meal);
        }

        // GET: Meal/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Meal meal = db.Meals.Find(id);
            if (meal == null)
            {
                return HttpNotFound();
            }
            return View(meal);
        }

        // POST: Meal/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Meal meal = db.Meals.Find(id);
            string imageDel = Path.Combine(Server.MapPath("~/Uploads/Meals"), db.Meals.Single(m => m.MealId == id).Image);
            FileInfo fi = new FileInfo(imageDel);
            if(fi != null)
            {
                System.IO.File.Delete(imageDel);
                fi.Delete();
            }
            db.Meals.Remove(meal);
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
