using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;

namespace TheHappyMeal.Models
{
    public class ReservationViewModel
    {
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
        public string BookDate { get; set; }

        [Required]
        public string BookTime { get; set; }

        public DateTime BookDateTime => BookTime.Length== 7 ? DateTime.ParseExact($"{BookDate} {BookTime}", "MM/dd/yyyy h : mm tt", CultureInfo.GetCultureInfo("en-US")) : DateTime.ParseExact($"{BookDate} {BookTime}", "MM/dd/yyyy hh:mm tt", CultureInfo.GetCultureInfo("en-US"));
    }
}