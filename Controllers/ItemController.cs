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
using System.Net.Mail;

namespace TheHappyMeal.Controllers
{
    [Authorize]
    public class ItemController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Item
        public async Task<ActionResult> Index()
        {
            var count = db.Items.Count() + 3;
            List<string> needList = new List<string>();
            for (int i = 3; i < count; i++)
            {
                var num = db.Items.Single(m => m.Item_Id == i).Number;
                var thr = db.Items.Single(m => m.Item_Id == i).Threshold;
                var name = db.Items.Single(m => m.Item_Id == i).Name;
                if (num < thr)
                {
                    needList.Add(name);
                }
            }
            if(needList.Count > 0)
            {
                MailMessage mail = new MailMessage("mohammadmayyaspr@gmail.com", "mohammadmayyascv@gmail.com");
                mail.Subject = "Warrning";
                mail.Body = "Hello, Mr.Mohammad we have shortage in this ingredients: " + "  ";

                foreach (var item in needList)
                {
                    mail.Body += item.ToUpper() + " ";
                }
                mail.IsBodyHtml = false;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = new System.Net.NetworkCredential("mohammadmayyaspr@gmail.com", "mohjal609");
                smtp.EnableSsl = true;
                smtp.Send(mail);

            }

            return View(await db.Items.ToListAsync());
        }

        // GET: Item/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ItemViewModel itemViewModel = await db.Items.FindAsync(id);
            if (itemViewModel == null)
            {
                return HttpNotFound();
            }
            return View(itemViewModel);
        }

        // GET: Item/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Item/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Item_Id,Name,Number,Threshold,Price,NofMeals")] ItemViewModel itemViewModel)
        {
            if (ModelState.IsValid)
            {
                db.Items.Add(itemViewModel);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(itemViewModel);
        }

        // GET: Item/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ItemViewModel itemViewModel = await db.Items.FindAsync(id);
            if (itemViewModel == null)
            {
                return HttpNotFound();
            }
           
            return View(itemViewModel);
        }

        // POST: Item/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Item_Id,Name,Number,Threshold,Price,NofMeals")] ItemViewModel itemViewModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(itemViewModel).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(itemViewModel);
        }

        // GET: Item/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ItemViewModel itemViewModel = await db.Items.FindAsync(id);
            if (itemViewModel == null)
            {
                return HttpNotFound();
            }
            return View(itemViewModel);
        }

        // POST: Item/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ItemViewModel itemViewModel = await db.Items.FindAsync(id);
            db.Items.Remove(itemViewModel);
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
