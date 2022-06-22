using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TheHappyMeal.ViewModels
{
    public class OrderViewModel
    {

        public DateTime Date { get; set; }

        [Display(Name = "Total Price")]
        public decimal TotalPrice { get; set; }

        [Display(Name = "Customer Name")]
        public string CustomerName { get; set; }

        public List<ListItems> Items { get; set; }
    } 

        public class ListItems
        {
            public int CategoryId { get; set; }
            public int MealID { get; set; }
            public decimal Price { get; set; }
            public int Quantity { get; set; }
            public decimal subTotal { get; set; }
        }
    
}