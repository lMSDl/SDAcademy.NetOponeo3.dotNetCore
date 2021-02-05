using System;
using System.Diagnostics;
using System.Linq;
using ConsoleApp.Configurations.Models;
using ConsoleApp.ConsoleServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Models;
using Newtonsoft.Json;
using Services.Fakers;
using Services.Fakers.Models;
using Services.Interfaces;

namespace ConsoleApp
{
    class Program
    {
        private static IConfigurationRoot ConfigurationRoot {get;}
        public static ConfigApp ConfigApp {get;}
        private static ICrudService<User> Service {get; set;}

        static Program()
        {
            ConfigurationRoot = GerConfigrationRoot();
            ConfigApp = new ConfigApp();
            //package Microsoft.Extensions.Configuration.Binder
            ConfigurationRoot.Bind(ConfigApp);
        }

        static void Main(string[] args)
        {
            ConfigDemo();

            DependencyInjectionDemo();

            //Service = new CrudService<User>(new UserFaker(), ConfigApp.Faker.NumberOfGeneratedObjects);
            //Service.Read().ToList().ForEach(x => System.Console.WriteLine(JsonConvert.SerializeObject(x)));
        }

        private static void DependencyInjectionDemo()
        {
            var serviceCollection = new ServiceCollection();
            var serviceProvider = serviceCollection
            .AddTransient<ConsoleWriteLineService>()
            .AddTransient<IConsoleService, ConsoleWriteLineService>()
            .AddTransient<IConsoleService, ConsoleWriteFiggleLineService>()
            .BuildServiceProvider();

            serviceProvider.GetServices<IConsoleService>().ToList().ForEach(x => x.WriteLine(x.GetType().Name));
            var consoleService = serviceProvider.GetService<IConsoleService>();
            consoleService.WriteLine("Hello");
        }

        private static void ScopeDemo()
        {
            var serviceCollection = new ServiceCollection();
            var serviceProvider = serviceCollection
            .AddScoped<IConsoleService, ConsoleWriteFiggleLineService>()
            .BuildServiceProvider();
            var scopeFactory = serviceProvider.GetRequiredService<IServiceScopeFactory>();
            IConsoleService instanceOne;
            IConsoleService instanceTwo;
            IConsoleService instanceThree;

            using (var scope = scopeFactory.CreateScope())
            {
                instanceOne = scope.ServiceProvider.GetService<IConsoleService>();
                instanceOne.WriteLine(nameof(instanceOne));
            }

            using (var scope = scopeFactory.CreateScope())
            {
                instanceTwo = scope.ServiceProvider.GetService<IConsoleService>();
                instanceThree = scope.ServiceProvider.GetService<IConsoleService>();
                instanceOne.WriteLine(nameof(instanceTwo));
            }

            Debug.Assert(instanceTwo == instanceThree);
            Debug.Assert(instanceOne != instanceTwo);
        }

        private static void ConfigDemo()
        {
            System.Console.WriteLine($"{ConfigurationRoot.GetSection("Section").GetSection("Subsection")["SubsectionKey2"]} {ConfigurationRoot.GetSection("Section")["SectionKey1"]}!");
            System.Console.WriteLine($"{ConfigApp.Section.Subsection.SubsectionKey2} {ConfigApp.Section.SectionKey1}!");
        }

        private static IConfigurationRoot GerConfigrationRoot()
        {
            //package Microsoft.Extensions.Configuration
            return new ConfigurationBuilder()
            //package Microsoft.Extensions.Configuration.FileExtensions
            //package Microsoft.Extensions.Configuration.json
            .AddJsonFile("Configurations/configApp.json", optional: true, reloadOnChange: true)
            //package Microsoft.Extensions.Configuration.ini
            .AddIniFile("Configurations/configApp.ini", optional: true, reloadOnChange: true)
            //package NetEscapades.Configuration.yaml
            .AddYamlFile("Configurations/configApp.yaml", optional: true, reloadOnChange: true)
            //package Microsoft.Extensions.Configuration.xml
            .AddXmlFile("Configurations/configApp.xml", optional: true, reloadOnChange: true)
            .AddYamlFile("Configurations/badconfig.yaml", optional: true, reloadOnChange: true)
            .Build();
        }
    }
}
