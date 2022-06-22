using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IdentitySample.Models;
using TheHappyMeal.Models;
using System.IO;

namespace TheHappyMeal.Controllers
{
    [Authorize(Roles = "Admin")]
    public class OfferController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Offer
        public async Task<ActionResult> Index()
        {
            return View(await db.Offers.ToListAsync());
        }

        // GET: Offer/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Offer offer= await db.Offers.FindAsync(id);
            if (offer == null)
            {
                return HttpNotFound();
            }
            return View(offer);
        }

        // GET: Offer/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Offer/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Offer_Id,Name,MealsName,OfferImage,OfferDescription,Price")] Offer offer, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                string path = Path.Combine(Server.MapPath("~/Uploads/Offers"), upload.FileName);
                upload.SaveAs(path);
                offer.OfferImage = upload.FileName;
                db.Offers.Add(offer);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(offer);
        }

        // GET: Offer/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Offer offer = await db.Offers.FindAsync(id);
            if (offer == null)
            {
                return HttpNotFound();
            }
            return View(offer);
        }

        // POST: Offer/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Offer_Id,Name,MealsName,OfferImage,OfferDescription,Price")] Offer offer, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                string oldPath = Path.Combine(Server.MapPath("~/Uploads/Offers"), offer.OfferImage);

                if (upload != null)
                {
                    System.IO.File.Delete(oldPath);
                    string path = Path.Combine(Server.MapPath("~/Uploads/Offers"), upload.FileName);
                    upload.SaveAs(path);
                    offer.OfferImage = upload.FileName;
                }
                db.Entry(offer).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(offer);
        }

        // GET: Offer/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Offer offer = await db.Offers.FindAsync(id);
            if (offer == null)
            {
                return HttpNotFound();
            }
            return View(offer);
        }

        // POST: Offer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Offer offer = await db.Offers.FindAsync(id);
            string imageDel = Path.Combine(Server.MapPath("~/Uploads/Offers"), db.Offers.Single(m => m.Offer_Id == id).OfferImage);
            FileInfo fi = new FileInfo(imageDel);
            if (fi != null)
            {
                System.IO.File.Delete(imageDel);
                fi.Delete();
            }
            db.Offers.Remove(offer);
            await db.SaveChangesAsync();
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
