using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace Synuit.Toolkit.SignalR.Test
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        const string URLS =
#if HTTPS
                        "http://0.0.0.0:15000;https://0.0.0.0:15001";
#else
                        "http://0.0.0.0:15000";
#endif

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseUrls(URLS)
                .UseStartup<Startup>();
    }
}
