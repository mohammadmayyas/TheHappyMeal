using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TheHappyMeal.Models
{

    [Table("Reservations")]
    public class Reservation
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public int Phone { get; set; }

        [Required]
        [Display(Name = "People Number")]
        public int PeopleNumber { get; set; }

        [Required]
        public DateTime BookDateTime { get; set; }

        [NotMapped]
        public string BookDate => BookDateTime.ToString("MM/dd/yyyy");

        [NotMapped]
        public string BookTime => BookDateTime.ToString("hh:mm tt");

    }

    [Table("Reviews")]
    public class Reviews
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Review { get; set; }

        public string Image { get; set; }

        public int Stars { get; set; }
    }

    [Table("Gallery")]
    public class Gallery
    {
        [Key]
        public int Id { get; set; }

        public string ImageName { get; set; }

        public string ImageCategory { get; set; }

    }

}
