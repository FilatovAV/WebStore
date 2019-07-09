using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

namespace WebStore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args).ConfigureLogging((host, log) =>
            {
                log.AddFilter<ConsoleLoggerProvider>("System", LogLevel.Error);
                log.AddFilter<ConsoleLoggerProvider>("Microsoft", LogLevel.Error);
            })
                //.UseUrls("http://0.0.0.0:0000") //Добавить на прослушку порт
                .UseStartup<Startup>();
    }
}
