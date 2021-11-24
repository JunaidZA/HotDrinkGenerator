using HotDrinkGenerator.Common.Enums;

namespace HotDrinkGenerator.Service.Interfaces
{
    public interface IHotDrinkService
    {
        public int BeansRemaining { get; set; }

        /// <summary>
        /// Create a hot drink of the specified type
        /// </summary>
        /// <param name="hotDrinkType"></param>
        /// <param name="addMilk"></param>
        /// <returns>A boolean specifying if the hot drink was successfully created</returns>
        bool CreateHotDrink(HotDrinkType hotDrinkType, bool addMilk);

        /// <summary>
        /// Checks if the required ingredients are available for the requested hot drink
        /// </summary>
        /// <param name="hotDrinkType"></param>
        /// <param name="addMilk"></param>
        /// <returns>A boolean specifying if the requested input parameters are valid for the selected drink</returns>
        bool AreIngredientsAvailableForHotDrink(HotDrinkType hotDrinkType, bool addMilk);

        /// <summary>
        /// Returns whether the remaining bean count is lower than a specific low threshold
        /// </summary>
        /// <returns>A boolean specifying if the bean count has reach a specific threshold</returns>
        bool AreBeansLow();

        /// <summary>
        /// Returns whether the hot drink type selected requires to ask the user if they would like milk
        /// </summary>
        /// <param name="hotDrinkType"></param>
        /// <returns>A boolean specifying if the hot drink type requires a milk input</returns>
        bool RequiresMilk(HotDrinkType hotDrinkType);
    }
}
