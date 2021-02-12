using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Models;
using Newtonsoft.Json;
using Services.Interfaces;

namespace ConsoleApp
{
    public class Startup
    {
        private ICrudServiceAsync<User> Service {get;}
        private ILogger Logger {get;}

        public Startup(ICrudServiceAsync<User> service, ILogger<Startup> logger)
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

            await Service.UpdateAsync(1000, null);
        }
    }
}