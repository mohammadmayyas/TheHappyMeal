using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IdentitySample.Models;
using TheHappyMeal.Models;

namespace TheHappyMeal.Controllers
{
    [Authorize(Roles ="Admin")]
    public class CuisineController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Cuisine
        public ActionResult Index()
        {
            return View(db.Cuisines.ToList());
        }

        // GET: Cuisine/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cuisine cuisine = db.Cuisines.Find(id);
            if (cuisine == null)
            {
                return HttpNotFound();
            }
            return View(cuisine);
        }

        // GET: Cuisine/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Cuisine/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CuisineId,Name")] Cuisine cuisine)
        {
            if (ModelState.IsValid)
            {
                db.Cuisines.Add(cuisine);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(cuisine);
        }

        // GET: Cuisine/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cuisine cuisine = db.Cuisines.Find(id);
            if (cuisine == null)
            {
                return HttpNotFound();
            }
            return View(cuisine);
        }

        // POST: Cuisine/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CuisineId,Name")] Cuisine cuisine)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cuisine).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cuisine);
        }

        // GET: Cuisine/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cuisine cuisine = db.Cuisines.Find(id);
            if (cuisine == null)
            {
                return HttpNotFound();
            }
            return View(cuisine);
        }

        // POST: Cuisine/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Cuisine cuisine = db.Cuisines.Find(id);
            db.Cuisines.Remove(cuisine);
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
