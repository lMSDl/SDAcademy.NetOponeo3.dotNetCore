using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Models;
using Newtonsoft.Json;
using Services.Interfaces;

namespace ConsoleApp
{
    public class Program
    {
        private ICrudServiceAsync<User> Service {get;}
        private ILogger Logger {get;}

        public Program(ICrudServiceAsync<User> service, ILogger<Program> logger)
        {
            Service = service;
            Logger = logger;
        }

        public async Task RunAsync() {
            using(Logger.BeginScope("Run")) {
                Logger.LogTrace($"Begin {nameof(RunAsync)}");
                
                using(Logger.BeginScope("Service ForEach"))
                    (await Service.ReadAsync()).ToList().ForEach(x => {
                        
                        Logger.LogInformation($"Display user {x.Id}");
                        System.Console.WriteLine(JsonConvert.SerializeObject(x)); 
                        }
                    );
                Logger.LogTrace($"End {nameof(RunAsync)}");
            }
        }
    }
}