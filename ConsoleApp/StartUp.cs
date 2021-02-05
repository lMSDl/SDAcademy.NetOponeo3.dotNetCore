﻿using System;
using System.Diagnostics;
using System.Linq;
using ConsoleApp.Configurations.Models;
using ConsoleApp.ConsoleServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Models;
using Newtonsoft.Json;
using Services.Fakers;
using Services.Fakers.Models;
using Services.Interfaces;

namespace ConsoleApp
{
    class StartUp
    {
        public static ServiceProvider ServiceProvider {get;}

        static StartUp()
        {
            var serviceCollection = new ServiceCollection();
            RegisterConfigrationRoot(serviceCollection);
            RegisterConfigApp(serviceCollection);
            serviceCollection
            .AddScoped<ICrudService<User>> (x => new CrudService<User>(new UserFaker(), x.GetService<ConfigApp>().Faker.NumberOfGeneratedObjects))
            .AddScoped<Program>()
            .AddLogging(builder => builder
                .AddConsole()
                .AddDebug()
            )
            .Configure<LoggerFilterOptions>(options => options.MinLevel = LogLevel.Trace);

            ServiceProvider = serviceCollection.BuildServiceProvider();
        }

        static void Main(string[] args)
        {
            ConfigDemo();
            DependencyInjectionDemo();

            //new Program(ServiceProvider.GetService<ICrudService<User>>()).Run();
            ServiceProvider.GetService<Program>().Run();
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
            var configurationRoot = ServiceProvider.GetService<IConfigurationRoot>();
            System.Console.WriteLine($"{configurationRoot.GetSection("Section").GetSection("Subsection")["SubsectionKey2"]} {configurationRoot.GetSection("Section")["SectionKey1"]}!");
            
            
            var configApp = ServiceProvider.GetService<ConfigApp>();
            System.Console.WriteLine($"{configApp.Section.Subsection.SubsectionKey2} {configApp.Section.SectionKey1}!");
        }

        private static void RegisterConfigrationRoot(ServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IConfigurationRoot>(x => 
                //package Microsoft.Extensions.Configuration
                new ConfigurationBuilder()
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
                .Build()
            );
        }
        private static void RegisterConfigApp(ServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<ConfigApp>(x => 
                {
                    var configApp = new ConfigApp();
                    //package Microsoft.Extensions.Configuration.Binder
                    x.GetService<IConfigurationRoot>().Bind(configApp);
                    return configApp;
                }
            );
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
