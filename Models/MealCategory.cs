using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheHappyMeal.Models
{
    public class MealCategory
    {

        public int Id { get; set; }
        public string Name { get; set; }

        public string CoverImage { get; set; }

        public virtual List<MealCategory> Categories { get; set; }
    }
}