using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TheHappyMeal.Models
{
    [Table("Items")]
    public class ItemViewModel
    {
        [Key]
        public int Item_Id { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        public int Number { get; set; }

        public int Threshold { get; set; }

        public float Price { get; set; }

        [Display(Name ="Number Of Meals Per Unit")]
        public int NofMeals { get; set; }


    }

    public class MailModel
    {
        public string From { get; set; }

        public string To { get; set; }

        public string Subject { get; set; }
        
        public string Body { get; set; }
        
    }
}