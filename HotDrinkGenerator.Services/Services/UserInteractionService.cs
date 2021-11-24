using HotDrinkGenerator.Common.Enums;
using HotDrinkGenerator.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HotDrinkGenerator.Service.Services
{
    /// <summary>
    /// A class to handle the user interface of a hot drink machine
    /// </summary>
    public class UserInteractionService : IUserInteractionService
    {
        private readonly IHotDrinkService _hotDrinkService;

        public UserInteractionService(IHotDrinkService hotDrinkService)
        {
            _hotDrinkService = hotDrinkService;
        }

        /// <inheritdoc/>
        public void RunUserInteraction()
        {
            var continueCreatingHotDrinks = true;
            while (continueCreatingHotDrinks)
            {
                Console.Clear();
                Console.WriteLine("Please enter the type of drink you would like (example: 2 for coffee):");

                var hotdrinkList = CreateOrderedHotDrinkList();

                foreach (var (itemNumber, hotDrinkType) in hotdrinkList)
                {
                    Console.WriteLine($"{itemNumber}. {hotDrinkType}");
                }

                var selectedHotDrinkType = HotDrinkInputHandler();

                try
                {
                    bool addMilk = false;
                    if (_hotDrinkService.RequiresMilk(selectedHotDrinkType))
                    {
                        addMilk = MilkInputHandler();
                    }

                    var sugar = SugarInputHandler();

                    if (_hotDrinkService.AreIngredientsAvailableForHotDrink(selectedHotDrinkType, addMilk) == false)
                    {
                        Console.WriteLine($"Sorry, there are not enough ingredients to make a {selectedHotDrinkType}.");
                        Console.WriteLine("Would you like to try another hot drink or turn off the machine? (yes/off)");
                        if (string.Equals(Console.ReadLine(), "off", StringComparison.InvariantCultureIgnoreCase))
                        {
                            continueCreatingHotDrinks = false;
                        }
                        continue;
                    }

                    var hotDrinkCreated = _hotDrinkService.CreateHotDrink(selectedHotDrinkType, addMilk);
                    if (!hotDrinkCreated)
                    {
                        throw new InvalidOperationException($"Something went wrong with the creation of the {selectedHotDrinkType}.");
                    }

                    var milkstring = CreateMilkOutputString(selectedHotDrinkType, addMilk);
                    var sugarString = CreateSugarOutputString(sugar);
                    Console.WriteLine($"{selectedHotDrinkType} with {milkstring}{(!string.IsNullOrEmpty(milkstring) ? " and " : string.Empty)}{sugarString} ready!");
                }
                catch
                {
                    Console.WriteLine($"Unable to make your {selectedHotDrinkType}, please try again later.");
                    break;
                }

                if (_hotDrinkService.AreBeansLow())
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write($"{_hotDrinkService.BeansRemaining} beans remaining, please consider refilling.");
                    Console.WriteLine();
                    Console.ResetColor();
                }

                Console.WriteLine("Would you like another hot drink? (enter 'off' to exit, or anything else to continue)");
                if (string.Equals(Console.ReadLine(), "off", StringComparison.InvariantCultureIgnoreCase))
                {
                    continueCreatingHotDrinks = false;
                }
            }
            Console.WriteLine($"Goodbye.");
        }

        /// <summary>
        /// Creates a string to output for relevant the amount of sugar
        /// </summary>
        /// <param name="sugar"></param>
        /// <returns>A string describing the sugar added</returns>
        public string CreateSugarOutputString(int sugar)
        {
            var sugarString = sugar > 0
                ? $"{sugar} {(sugar == 1 ? "teaspoon" : "teaspoons")}"
                : "no";
            sugarString += " sugar";
            return sugarString;
        }

        /// <summary>
        /// Creates a string to output for relevant hotdrink type and whether milk is added
        /// </summary>
        /// <param name="selectedHotDrinkType"></param>
        /// <param name="addMilk"></param>
        /// <returns>A string describing if milk was added</returns>
        public string CreateMilkOutputString(HotDrinkType selectedHotDrinkType, bool addMilk)
        {
            var hotDrinkstring = selectedHotDrinkType == HotDrinkType.Coffee
                ? $"{(addMilk == false ? "no " : string.Empty)}milk"
                : string.Empty;
            return hotDrinkstring;
        }

        /// <summary>
        /// Handle the user selection of the drink
        /// </summary>
        /// <returns>A HotDrink type of the selected drink</returns>
        private static HotDrinkType HotDrinkInputHandler()
        {
            HotDrinkType selectedHotDrinkType = 0;
            var isValidHotDrinkInput = false;
            while (!isValidHotDrinkInput)
            {
                string hotDrinkSelectionInput = Console.ReadLine();
                if ((Enum.TryParse(hotDrinkSelectionInput, true, out selectedHotDrinkType)) && Enum.IsDefined(typeof(HotDrinkType), selectedHotDrinkType))
                {
                    Console.WriteLine($"{selectedHotDrinkType} chosen.");
                    isValidHotDrinkInput = true;
                }
                else
                {
                    Console.WriteLine("Invalid selection. Please enter a valid selection.");
                    isValidHotDrinkInput = false;
                }
            }
            return selectedHotDrinkType;
        }

        /// <summary>
        /// Handle the user input of the milk
        /// </summary>
        /// <returns>Boolean whether milk is requested to be added</returns>
        private static bool MilkInputHandler()
        {
            bool addMilk = false;
            var isValidMilkInput = false;
            while (!isValidMilkInput)
            {
                Console.WriteLine("Would you like to add milk (yes/no)?");
                var milkInput = Console.ReadLine();
                if (string.Equals(milkInput, "yes", StringComparison.InvariantCultureIgnoreCase))
                {
                    addMilk = true;
                    isValidMilkInput = true;
                }
                else if (string.Equals(milkInput, "no", StringComparison.InvariantCultureIgnoreCase))
                {
                    addMilk = false;
                    isValidMilkInput = true;
                }
                else
                {
                    Console.WriteLine("Please enter 'yes' or 'no' only.");
                    isValidMilkInput = false;
                }
            }
            return addMilk;
        }

        /// <summary>
        /// Handle the user input of the sugar required
        /// </summary>
        /// <returns>Amount of sugar needed</returns>
        private static int SugarInputHandler()
        {
            var isValidSugarInput = false;
            var sugar = 0;
            while (!isValidSugarInput)
            {
                Console.WriteLine("Please enter the amount of teaspoons of sugar to use (example: 3):");
                var sugarInput = Console.ReadLine();
                isValidSugarInput = int.TryParse(sugarInput, out sugar);
                if (!isValidSugarInput)
                {
                    Console.WriteLine("Please enter a valid number (example: 3):");
                }
            }
            return sugar;
        }

        /// <summary>
        /// Parse the HotDrinkType enum into an ordered list for display
        /// </summary>
        /// <returns></returns>
        private static List<(int itemNumber, HotDrinkType hotDrinkType)> CreateOrderedHotDrinkList ()
        {
            var hotdrinkList = Enum.GetValues(typeof(HotDrinkType))
                    .Cast<HotDrinkType>()
                    .Select(hotDrinkType => ((int)hotDrinkType, hotDrinkType))
                    .OrderBy(hotDrinkType => hotDrinkType.Item1)
                    .ToList();

            return hotdrinkList;
        }
    }
}
