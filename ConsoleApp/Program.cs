using System;
using System.Linq;
using ConsoleApp.Configurations.Models;
using Microsoft.Extensions.Configuration;
using Models;
using Newtonsoft.Json;
using Services.Fakers;
using Services.Fakers.Models;
using Services.Interfaces;

namespace ConsoleApp
{
    class Program
    {
        private static ICrudService<User> Service {get; set;}

        static void Main(string[] args)
        {
            //package Microsoft.Extensions.Configuration
            var config = new ConfigurationBuilder()
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

            var configApp = new ConfigApp();
            //package Microsoft.Extensions.Configuration.Binder
            config.Bind(configApp);

            System.Console.WriteLine($"{config.GetSection("Section").GetSection("Subsection")["SubsectionKey2"]} {config.GetSection("Section")["SectionKey1"]}!");
            System.Console.WriteLine($"{configApp.Section.Subsection.SubsectionKey2} {configApp.Section.SectionKey1}!");



            Service = new CrudService<User>(new UserFaker(), configApp.Faker.NumberOfGeneratedObjects);
            Service.Read().ToList().ForEach(x => System.Console.WriteLine( JsonConvert.SerializeObject(x) ));
        }
    }
}
