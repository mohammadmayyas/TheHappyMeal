using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheHappyMeal.Models
{
    public class OrderDetails
    {
        public OrderDetails()
        {
            Date = DateTime.Now;
        }

        public int ID { get; set; }
        public int OrderID { get; set; }
        public int MealID { get; set; }
        public decimal Price { get; set; }
        public decimal Profit { get; set; }
        public Nullable<DateTime> Date { get; set; }
        public int Quantity { get; set; }
        public decimal subTotal { get; set; }
        public string Status { get; set; }

        [ForeignKey("OrderID")]
        public virtual Order Order { get; set; }

        [ForeignKey("MealID")]
        public virtual Meal Meal { get; set; }


    }
}