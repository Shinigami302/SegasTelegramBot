using System.Threading;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace SegasTelegramBotWebApplicationSP
{
    public class Program
    {
        
        public static void Main(string[] args)
        {
            InitBot(BotHome.GetBotHomeInstance);
            CreateWebHostBuilder(args).Build().Run();
            StopBot(BotHome.GetBotHomeInstance);
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();

        private static void InitBot(BotHome botHome)
        {
            new Thread(new ThreadStart(botHome.BotInit)).Start();
        }

        private static void StopBot(BotHome botHome)
        {
            botHome.BotStopReceiving();
        }
        
    }
}
