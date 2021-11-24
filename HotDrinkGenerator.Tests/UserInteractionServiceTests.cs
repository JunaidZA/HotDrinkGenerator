using HotDrinkGenerator.Common.Enums;
using HotDrinkGenerator.Service.Services;
using NUnit.Framework;
using NSubstitute;
using HotDrinkGenerator.Service.Interfaces;

namespace HotDrinkGenerator.Tests
{
    [TestFixture]
    public class UserInteractionServiceTests
    {
        private UserInteractionService _userInteractionService;
        private IHotDrinkService _hotDrinkService;

        [SetUp]
        public void Setup()
        {
            _hotDrinkService = Substitute.For<IHotDrinkService>();
            _userInteractionService = new UserInteractionService(_hotDrinkService);
        }

        [TestCase(1, "1 teaspoon sugar")]
        [TestCase(2, "2 teaspoons sugar")]
        [TestCase(999, "999 teaspoons sugar")]
        [TestCase(0, "no sugar")]
        public void GivenAmountOfSugar_CreateSugarOutputString_ShouldReturnAValidString(int sugar, string expectedString)
        {
            //Arrange
            //Act
            var result = _userInteractionService.CreateSugarOutputString(sugar);

            //Assert
            Assert.AreEqual(expectedString, result);
        }

        [TestCase(HotDrinkType.Coffee, true, "milk")]
        [TestCase(HotDrinkType.Coffee, false, "no milk")]
        [TestCase(HotDrinkType.Cappuccino, true, "")]
        [TestCase(HotDrinkType.Latte, true, "")]
        public void GivenAmountOfMilk_CreateMilkOutputString_ShouldReturnAValidString(HotDrinkType selectedHotDrinkType, bool addMilk, string expectedString)
        {
            //Arrange
            //Act
            var result = _userInteractionService.CreateMilkOutputString(selectedHotDrinkType, addMilk);

            //Assert
            Assert.AreEqual(expectedString, result);
        }
    }
}
