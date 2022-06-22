using CrystalDecisions.CrystalReports.Engine;
using IdentitySample.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using TheHappyMeal.Models;
using TheHappyMeal.ViewModels;
using System.IO;
using System.Data.Entity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;

namespace TheHappyMeal.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public OrdersController()
        {
        }

        public OrdersController(ApplicationUserManager userManager, ApplicationRoleManager roleManager)
        {
            UserManager = userManager;
            RoleManager = roleManager;
        }

        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        private ApplicationRoleManager _roleManager;
        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }

        public ActionResult Index()
        {
            return View(db.Orders.ToList());
        }

        //Extarct orders reports by crystal report
        public ActionResult ExportOrderReport(DateTime start, DateTime end)
        {
            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Reports/OrdersReport.rpt")));

            if (start == null && end == null) { return View("Index"); }
            var orders = db.Orders.Where(x => x.Date >= start && x.Date <= end && x.Status == "delivered");
            rd.SetDataSource(orders.ToList());
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();

            Stream str = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            str.Seek(0, SeekOrigin.Begin);
            string fileName = string.Format("Orders..{0}", DateTime.Now);
            return File(str, "application/pdf", fileName);
        }

        //Extarct sales reports by crystal report
        public ActionResult ExportSalesReport(DateTime start, DateTime end)
        {
            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Reports/SalesReport.rpt")));

            if (start == null && end == null) { return View("Index"); }
            var orders = db.OrderDetails.Where(x => x.Date >= start && x.Date <= end && x.Status == "ready");
            rd.SetDataSource(orders.ToList());
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();

            Stream str = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            str.Seek(0, SeekOrigin.Begin);
            string fileName = string.Format("Sales..{0}", DateTime.Now);
            return File(str, "application/pdf", fileName);
        }

        // GET: Orders/Reports
        public ActionResult Reports()
        {
            return View();
        }

        //GET: Orders/DeliveryView
        public ActionResult DeliveryView()
        {
            var delveryName = User.Identity.Name;
            DateTime sutDate = DateTime.Now.AddDays(-1);
            var delveryOrders = db.Orders.Where(m => (m.DeliveryName == delveryName && m.Date >= sutDate)  && m.Status != "delivered");
            return View(delveryOrders.Take(1).ToList());
        }

        // GET: Orders
        public ActionResult AddOrder()
        {
            ViewBag.CategoryList = new SelectList(db.Categories, "Id", "Name");
            ViewBag.MealsList = new SelectList(db.Meals, "MealId", "Name");
            return View();
        }

        //Check if the customer name is exist or not.
        public JsonResult CheckCustomerName(string username)
        {
            bool result = !db.Users.ToList().Exists(x => x.UserName.Equals(username, StringComparison.CurrentCultureIgnoreCase));
            return Json(result);
        }

        public JsonResult GetMealsByCatyId(int ID)
        {
            db.Configuration.ProxyCreationEnabled = false;
            return Json(db.Meals.Where(m => m.CategoryId == ID).ToList(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult getPriceByMealId(int mealId)
        {
            db.Configuration.ProxyCreationEnabled = false;
            decimal mealPrice = db.Meals.Single(m => m.MealId == mealId).Price;
            return Json(mealPrice, JsonRequestBehavior.AllowGet);
        }

        // Set: Orders/
        [HttpPost]
        public JsonResult AddOrderAndOrderDetials(OrderViewModel orderViewModel)
        {
            bool status = true;

            var isValidModel = TryUpdateModel(orderViewModel);
            var customerName = User.Identity.Name;
            var waitTime = 20;
            DateTime date = DateTime.Now;
            DateTime date2 = date.AddHours(-1);

            //Cashed the delivery employee names 
            Queue<string> deliveries = new Queue<string>();
            var role = RoleManager.FindById("4ed421d7-3f17-4943-8f1a-f5fde172efe1");
            // Get the list of Users in this Role
            var users = new List<ApplicationUser>();
            if(HttpContext.Cache["var"] == null)
            {
                // Get the list of Users in this Role
                foreach (var user in UserManager.Users.ToList())
                {
                    if (UserManager.IsInRole(user.Id, role.Name))
                    {
                        users.Add(user);
                    }
                }
                HttpContext.Cache["var"] = users;
            }

            users = (List<ApplicationUser>)HttpContext.Cache["var"];
            if (users.Count() == 0)
            {
                foreach (var user in UserManager.Users.ToList())
                {
                    if (UserManager.IsInRole(user.Id, role.Name))
                    {
                        users.Add(user);
                    }
                }
            }

            foreach (var item in users)
            {
                deliveries.Enqueue(item.UserName);
            }
            var deliveyName = deliveries.Dequeue();
            users.RemoveAt(0);

            //Save order and order details
            if (isValidModel)
            {
                using (db)
                {
                    Order order = new Order()
                    {
                        Date = DateTime.Now,
                        CustomerName = orderViewModel.CustomerName,
                        TotalPrice = orderViewModel.TotalPrice,
                        Address = db.Users.Single(m => m.UserName == customerName).Address,
                        PhoneNumber = db.Users.Single(m => m.UserName == customerName).PhoneNumber,
                        DeliveryName = deliveyName,
                        Status = "pending"

                    };
                    db.Orders.Add(order);

                    if (db.SaveChanges() > 0)
                    {
                        int orderID = db.Orders.Max(o => o.ID);

                        foreach (var item in orderViewModel.Items)
                        {
                            var profit = db.Meals.Single(m => m.MealId == item.MealID).Profit;
                            OrderDetails orderDetails = new OrderDetails()
                            {
                                OrderID = orderID,
                                MealID = item.MealID,
                                Price = item.Price,
                                Quantity = item.Quantity,
                                subTotal = item.subTotal,
                                Profit = profit,
                                Date = DateTime.Now,
                                Status = "pending"
                            };
                            db.OrderDetails.Add(orderDetails);

                        }

                        var ordersCount = db.Orders.Where(m => m.Date >= date2 && m.Date <= date).Count();
                        waitTime += ordersCount * 2;

                        if (db.SaveChanges() > 0)
                        {
                            return new JsonResult { Data = new { status = status, message = "Your Order Added Successfully, will be ready in" + " " + waitTime + " " + "minutes" } };
                        }
                    }
                }
            }

            status = false;
            return new JsonResult { Data = new { status = status, message = "Error !" } };
        }

        // GET: Orders1/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders1/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Date,TotalPrice,CustomerName,Address,PhoneNumber,DeliveryName,Status")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("DeliveryView");
            }
            return View(order);
        }

        // GET: Order/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Meal/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Order order = db.Orders.Find(id);
            db.Orders.Remove(order);
            db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}