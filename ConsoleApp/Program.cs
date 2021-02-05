using System;
using System.Linq;
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
            Service = new CrudService<User>(new UserFaker() , 15);

            Service.Read().ToList().ForEach(x => System.Console.WriteLine( JsonConvert.SerializeObject(x) ));
        }
    }
}
