using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TheHappyMeal.Models
{

    [Table("Offers")]
    public class Offer
    {
        public Offer()
        {
            this.Meals = new HashSet<Meal>();
        }


        [Key]
        public int Offer_Id { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        [Display(Name = "Meals Name")]
        [StringLength(50)]
        public string MealsName { get; set; }

        [Display(Name = "Offer Image")]
        public string OfferImage { get; set; }

        [StringLength(256)]
        [Display(Name = "Offer Description")]
        public string OfferDescription { get; set; }

        [Required]
        public decimal Price { get; set; }

        public virtual ICollection<Meal> Meals { get; set; }

    }
}