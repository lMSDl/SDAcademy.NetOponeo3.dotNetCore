using System.Linq;
using Microsoft.Extensions.Logging;
using Models;
using Newtonsoft.Json;
using Services.Interfaces;

namespace ConsoleApp
{
    public class Program
    {
        private ICrudService<User> Service {get;}
        private ILogger Logger {get;}

        public Program(ICrudService<User> service, ILogger<Program> logger)
        {
            Service = service;
            Logger = logger;
        }

        public void Run() {
            Logger.LogTrace($"Begin {nameof(Run)}");
            Service.Read().ToList().ForEach(x => {
                
                Logger.LogInformation($"Display user {x.Id}");
                System.Console.WriteLine(JsonConvert.SerializeObject(x)); 
                }
            );
            Logger.LogTrace($"End {nameof(Run)}");
        }
    }
}