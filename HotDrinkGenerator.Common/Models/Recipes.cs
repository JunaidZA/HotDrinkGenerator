using HotDrinkGenerator.Common.Enums;
using System.Collections.Generic;

namespace HotDrinkGenerator.Common.Models
{
    public class Recipes 
    {
        public readonly Dictionary<HotDrinkType, Ingredients> HotDrinkRecipes = new()
        {
            { HotDrinkType.Coffee, new Ingredients { Beans = 2, ShouldRequestMilk = true, Milk = 1 } },
            { HotDrinkType.Cappuccino, new Ingredients { Beans = 5, ShouldRequestMilk = false, Milk = 3 } },
            { HotDrinkType.Latte, new Ingredients { Beans = 3, ShouldRequestMilk = false, Milk = 2 } }
        };
    }
}
