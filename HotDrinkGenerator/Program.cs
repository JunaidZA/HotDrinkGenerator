using HotDrinkGenerator.Service.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using HotDrinkGenerator.Application.DependencyInjection;

namespace HotDrinkGenerator.Application
{
    class Program
    {
        public static void Main()
        {
            var serviceProvider = DependencyInjectionHandler.RegisterServices();
            var userInteractionService = serviceProvider.GetService<IUserInteractionService>();

            userInteractionService.RunUserInteraction();

            DependencyInjectionHandler.DisposeServices(serviceProvider);
        }
    }
}
