using IdentitySample.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TheHappyMeal.Models
{
    public class Order
    {
        public int ID { get; set; }

        public DateTime Date { get; set; }

        [Display(Name = "Total Price")]
        public decimal TotalPrice { get; set; }

        [Display(Name = "Customer Name")]
        public string CustomerName { get; set; }

        public string Address { get; set; }

        public string PhoneNumber { get; set; }

        public string DeliveryName { get; set; }

        public string Status { get; set; }

        public virtual List<OrderDetails> OrderDetailsList { get; set; }
    }

}