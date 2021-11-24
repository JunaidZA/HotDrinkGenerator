using HotDrinkGenerator.Common.Enums;
using HotDrinkGenerator.Common.Models;
using HotDrinkGenerator.Service.Interfaces;
using Microsoft.Extensions.Configuration;

namespace HotDrinkGenerator.Service.Services
{
    /// <summary>
    /// A class handling the internal workings of creating a hot drink
    /// </summary>
    public class HotDrinkService : IHotDrinkService
    {
        private readonly IConfigurationRoot _configuration;
        private readonly Recipes _recipes;
        private readonly int _beansLowWarningThreshold;

        public int BeansRemaining { get; set; }
        public int MilkRemaining { get; set; }

        public HotDrinkService(IConfigurationRoot configuration, Recipes recipes)
        {
            _configuration = configuration;
            _recipes = recipes;

            if (int.TryParse(_configuration.GetSection("GeneratorOptions")["MaximumBeanCapacity"], out var maxBeans))
            {
                BeansRemaining = maxBeans;
            }
            if (int.TryParse(_configuration.GetSection("GeneratorOptions")["MaximumMilkCapacity"], out var maxMilk))
            {
                MilkRemaining = maxMilk;
            }

            _ = int.TryParse(_configuration.GetSection("GeneratorOptions")["BeansLowWarningThreshold"], out _beansLowWarningThreshold);
        }

        /// <inheritdoc/>
        public bool CreateHotDrink(HotDrinkType hotDrinkType, bool addMilk)
        {
            var ingredients = _recipes.HotDrinkRecipes[hotDrinkType];

            BeansRemaining -= ingredients.Beans;
            if (addMilk)
            {
                MilkRemaining -= ingredients.Milk;
            }

            return BeansRemaining >= 0 && MilkRemaining >= 0;
        }

        /// <inheritdoc/>
        public bool AreIngredientsAvailableForHotDrink(HotDrinkType hotDrinkType, bool addMilk)
        {
            var ingredients = _recipes.HotDrinkRecipes[hotDrinkType];

            if (BeansRemaining < ingredients.Beans)
            {
                return false;
            }

            if (RequiresMilk(hotDrinkType) == false || (RequiresMilk(hotDrinkType) && addMilk))
            {
                if (MilkRemaining < ingredients.Milk)
                {
                    return false;
                }
            }

            return true;
        }

        /// <inheritdoc/>
        public bool AreBeansLow()
        {
            return BeansRemaining <= _beansLowWarningThreshold;
        }

        /// <inheritdoc/>
        public bool RequiresMilk(HotDrinkType hotDrinkType)
        {
            return _recipes.HotDrinkRecipes[hotDrinkType].ShouldRequestMilk;
        }
    }
}
