using System.Linq;
using Models;
using Newtonsoft.Json;
using Services.Interfaces;

namespace ConsoleApp
{
    public class Program
    {
        private ICrudService<User> Service {get;}

        public Program(ICrudService<User> service)
        {
            Service = service;
        }

        public void Run() {
            Service.Read().ToList().ForEach(x => System.Console.WriteLine(JsonConvert.SerializeObject(x)));
        }
    }
}