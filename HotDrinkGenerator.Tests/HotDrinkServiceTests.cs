using HotDrinkGenerator.Common.Enums;
using HotDrinkGenerator.Service.Services;
using NUnit.Framework;
using Microsoft.Extensions.Configuration;
using NSubstitute;
using HotDrinkGenerator.Common.Models;

namespace HotDrinkGenerator.Tests
{
    [TestFixture]
    public class HotDrinkServiceTests
    {
        private HotDrinkService _hotDrinkService;
        private IConfigurationRoot _configurationRoot;
        private Recipes _recipes;

        [SetUp]
        public void Setup()
        {
            var MaximumBeanCapacity = 25;
            var MaximumMilkCapacity = 20;
            var BeansLowWarningThreshold = 5;
            _configurationRoot = Substitute.For<IConfigurationRoot>();
            _configurationRoot.GetSection("GeneratorOptions")["MaximumBeanCapacity"].Returns(MaximumBeanCapacity.ToString());
            _configurationRoot.GetSection("GeneratorOptions")["MaximumMilkCapacity"].Returns(MaximumMilkCapacity.ToString());
            _configurationRoot.GetSection("GeneratorOptions")["BeansLowWarningThreshold"].Returns(BeansLowWarningThreshold.ToString());
            _recipes = Substitute.For<Recipes>();
            _hotDrinkService = new HotDrinkService(_configurationRoot, _recipes);
        }

        [TestCase(HotDrinkType.Cappuccino, true)]
        [TestCase(HotDrinkType.Cappuccino, false)]
        [TestCase(HotDrinkType.Coffee, true)]
        [TestCase(HotDrinkType.Coffee, false)]
        [TestCase(HotDrinkType.Latte, true)]
        [TestCase(HotDrinkType.Latte, false)]
        public void GivenAvailableIngredients_AreIngredientsAvailableForHotDrink_ShouldReturnTrue(HotDrinkType hotDrinkType, bool addMilk)
        {
            //Arrange
            _hotDrinkService.BeansRemaining = 25;
            _hotDrinkService.MilkRemaining = 20;

            //Act
            var result = _hotDrinkService.AreIngredientsAvailableForHotDrink(hotDrinkType, addMilk);

            //Assert
            Assert.AreEqual(true, result);
        }

        [TestCase(HotDrinkType.Cappuccino, true, 5, 6, true)]
        [TestCase(HotDrinkType.Cappuccino, true, 4, 6, false)]
        [TestCase(HotDrinkType.Cappuccino, true, 5, 1, false)]
        [TestCase(HotDrinkType.Coffee, true, 5, 0, false)]
        [TestCase(HotDrinkType.Coffee, false, 5, 0, true)]
        public void GivenUnavailableIngredients_AreIngredientsAvailableForHotDrink_ShouldReturnFalse(HotDrinkType hotDrinkType, bool addMilk, int beansRemaining, int milkRemaining, bool expectedResult)
        {
            //Arrange
            _hotDrinkService.BeansRemaining = beansRemaining;
            _hotDrinkService.MilkRemaining = milkRemaining;

            //Act
            var result = _hotDrinkService.AreIngredientsAvailableForHotDrink(hotDrinkType, addMilk);

            //Assert
            Assert.AreEqual(expectedResult, result);
        }

        [TestCase(6, false)]
        [TestCase(4, true)]
        public void GivenRemainingBeans_AreBeansLow_ShouldReturnIfBeansAreLow(int beansRemaining, bool expectedResult)
        {
            //Arrange
            _hotDrinkService.BeansRemaining = beansRemaining;

            //Act
            var result = _hotDrinkService.AreBeansLow();

            //Assert
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void GivenValidParameters_CreateHotDrink_ShouldConsumeTheCorrectAmountOfIngredients()
        {
            //Arrange
            var hotDrinkType = HotDrinkType.Cappuccino;
            var addMilk = true;
            var ingredients = _recipes.HotDrinkRecipes[hotDrinkType];
            _ = int.TryParse(_configurationRoot.GetSection("GeneratorOptions")["MaximumBeanCapacity"], out var maxBeans);
            _ = int.TryParse(_configurationRoot.GetSection("GeneratorOptions")["MaximumMilkCapacity"], out var maxMilk);

            //Act
            _hotDrinkService.CreateHotDrink(hotDrinkType, addMilk);

            //Assert
            var actualBeansRemaing = _hotDrinkService.BeansRemaining;
            var expectedBeansRemaining = maxBeans - ingredients.Beans;
            Assert.AreEqual(expectedBeansRemaining, actualBeansRemaing);

            var actualMilkRemaing = _hotDrinkService.MilkRemaining;
            var expectedMilkRemaining = maxMilk - ingredients.Milk;
            Assert.AreEqual(expectedMilkRemaining, actualMilkRemaing);
        }
    }
}
