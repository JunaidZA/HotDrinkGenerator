using System;
using HotDrinkGenerator.Service.Interfaces;
using HotDrinkGenerator.Service.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System.IO;
using HotDrinkGenerator.Common.Models;

namespace HotDrinkGenerator.Application.DependencyInjection
{
    class DependencyInjectionHandler
    {
        public static ServiceProvider RegisterServices()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddTransient<IHotDrinkService, HotDrinkService>();
            serviceCollection.AddTransient<IUserInteractionService, UserInteractionService>();
            serviceCollection.AddSingleton<Recipes>();

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                .AddJsonFile("appsettings.json", false)
                .Build();

            serviceCollection.AddSingleton(configuration);

            var serviceProvider = serviceCollection.BuildServiceProvider();
            return serviceProvider;
        }

        public static void DisposeServices(ServiceProvider serviceProvider)
        {
            if (serviceProvider == null)
            {
                return;
            }
            if (serviceProvider is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }
    }
}
