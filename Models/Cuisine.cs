using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheHappyMeal.Models
{
    public class Cuisine
    {
        public int CuisineId { get; set; }
        public string Name { get; set; }

        public virtual List<Meal> Meals { get; set; }
    }
}