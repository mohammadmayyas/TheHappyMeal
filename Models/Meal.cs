using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TheHappyMeal.Models
{
    [Table("Meals")]
    public class Meal
    {
        public Meal()
        {
            this.Offers = new HashSet<Offer>();
        }

        [Key]
        public int MealId { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [Required]
        public decimal Profit { get; set; }

        [Required]
        public decimal Price { get; set; }
        public string Image { get; set; }
        public int CategoryId { get; set; }
        public int CuisineId { get; set; }

        [StringLength(256)]
        public string Ingredients { get; set; }

        [Required]
        [Display(Name = "Preparation Time")]
        public int PreparationTime { get; set; }

        [ForeignKey("CategoryId")]
        public virtual MealCategory Category { get; set; }

        [ForeignKey("CuisineId")]
        public virtual Cuisine Cuisines { get; set; }

        public virtual List<OrderDetails> OrderDetailsList { get; set; }

        public virtual ICollection<Offer> Offers { get; set; }


    }
}