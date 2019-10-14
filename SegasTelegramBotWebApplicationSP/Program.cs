using System;
using System.Threading;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SegasTelegramBotWebApplicationSP.Models;

namespace SegasTelegramBotWebApplicationSP
{
    public class Program
    {
        
        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();
            var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;
            SegasBotContext ctx = services.GetRequiredService<SegasBotContext>();
            InitBot(BotHome.GetBotHomeInstance, ctx);

            //try
            //{
            //    var ctx = services.GetRequiredService<SegasBotContext>();
            //    var testItem = new SBCommands() { Answers = "1" };
            //    ctx.SBCommands.Add(testItem);
            //    ctx.SaveChanges();
            //}
            //catch (Exception ex)
            //{

            //    var logger = services.GetRequiredService<ILogger<Program>>();
            //    logger.LogError(ex, "An error occurred while seeding the database.");
            //}
            CreateWebHostBuilder(args).Build().Run();
            StopBot(BotHome.GetBotHomeInstance);
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();

        private static void InitBot(BotHome botHome, SegasBotContext context)
        {
            botHome.BotInit(context);
        }

        private static void StopBot(BotHome botHome)
        {
            botHome.BotStopReceiving();
        }
        
    }
}
