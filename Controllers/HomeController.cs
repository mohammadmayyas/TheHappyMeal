using IdentitySample.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using TheHappyMeal.Models;
using System.Net.Mail;

namespace IdentitySample.Controllers
{
   
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            var count = 0;

            if (Request.IsAuthenticated && (User.IsInRole("Admin") || User.IsInRole("Chef") || User.IsInRole("Call Center") || User.IsInRole("Delivery")) )
            {
                count = 1;
            }
            switch (count)
            {
                case 1:
                    return RedirectToAction("Welcome", "UsersAdmin");
                default:
                    return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(ReservationViewModel model)
        {
            if (ModelState.IsValid)
            {
                Reservation ob = new Reservation() { Name = model.Name, Email = model.Email, Phone = model.Phone, BookDateTime = model.BookDateTime };
                db.Reservations.Add(ob);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public ActionResult ReservationIndex()
        {
            return View(db.Reservations.ToList());
        }

        // GET: Home/ReservationCreate
        public ActionResult ReservationCreate()
        {
            return View();
        }

        // POST: Home/ReservationCreate
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReservationCreate(ReservationViewModel model)
        {
            if (ModelState.IsValid)
            {
                Reservation ob = new Reservation() { Name = model.Name, Email = model.Email, Phone = model.Phone, BookDateTime = model.BookDateTime };
                db.Reservations.Add(ob);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        // GET: Home/Edit/5
        public ActionResult ReservationEdit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservation reservation = db.Reservations.Find(id);
            if (reservation == null)
            {
                return HttpNotFound();
            }
            return View(reservation);
        }

        // POST: Home/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReservationEdit([Bind(Include = "Id,Name,Email,Phone,BookDateTime,PeopleNumber")] Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(reservation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ReservationIndex");
            }
            return View(reservation);
        }

        // GET: Home/ReservationDelete/5
        public ActionResult ReservationDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservation reservation = db.Reservations.Find(id);
            if (reservation == null)
            {
                return HttpNotFound();
            }
            return View(reservation);
        }

        // POST: Home/ReservationDelete/5
        [HttpPost, ActionName("ReservationDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult ReservationDeleteConfirmed(int id)
        {
            Reservation reservation = db.Reservations.Find(id);
            db.Reservations.Remove(reservation);
            db.SaveChangesAsync();
            return RedirectToAction("ReservationIndex");
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Contact(MailModel model)
        {
            MailMessage mail = new MailMessage(model.From, "mohammadmayyascv@gmail.com");
            mail.Subject = model.Subject;
            mail.Body = model.Body;
            mail.IsBodyHtml = false;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.UseDefaultCredentials = true;
            smtp.Credentials = new System.Net.NetworkCredential("mohammadmayyaspr@gmail.com", "mohjal609");
            smtp.EnableSsl = true;
            smtp.Send(mail);
            ViewBag.message = "The message has been sent";
            return View();
        }

        [AllowAnonymous]
        public ActionResult Menu()
        {
            ViewBag.MealsList = new SelectList(db.Meals, "MealId", "Name");
            ViewBag.CategoryList = new SelectList(db.Categories, "Id", "Name");
            ViewBag.CuisineList = new SelectList(db.Cuisines, "CuisineId", "Name");
            ViewBag.IngredientList = new SelectList(db.Items, "Item_Id", "Name");
            return View(db.Meals.ToList());
        }

        [AllowAnonymous]
        public ActionResult Offers()
        {
            return View(db.Offers.ToList());
        }

        public ActionResult Gallery()
        {
            return View(db.Gallery.ToList());
        }


    }
}
