using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace SegasTelegramBotWebApplicationSP
{
    public class Program
    {
        private static readonly TelegramBotClient Bot = new TelegramBotClient("962554948:AAHh6J2clB-gNm4vIemRgXk9NxdET4ZobG4") { Timeout = TimeSpan.FromSeconds(10) };
        public static void Main(string[] args)
        {
            InitBot();
            CreateWebHostBuilder(args).Build().Run();
            Bot.StopReceiving();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();

        private static void InitBot()
        {
            //var me = Bot.GetMeAsync().Result;

            Bot.OnMessage += BotOnMessageReceived;
            Bot.StartReceiving(Array.Empty<UpdateType>());
        }

        private static async void BotOnMessageReceived(object sender, MessageEventArgs messageEventArgs)
        {
            var message = messageEventArgs.Message;
            //Console.WriteLine(message.From.FirstName + " : " + message.Text);
            if (message == null || message.Type != MessageType.Text) return;

            switch (message.Text)
            {
                case "/start":
                    await Bot.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);
                    await Task.Delay(500);
                    await Bot.SendTextMessageAsync(
                                    message.Chat.Id, @"Поки не так багато варіантів: 
                        /weather - погода у Львові 
                        /roll - рулетка
                        /exchange - курс валют");
                    break;
                case "/roll":
                    Roll(message);
                    break;
                case "/weather":
                    await Bot.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);
                    Forecast forecast = new Forecast();
                    String s = String.Empty;
                    Dictionary<string, string> result = forecast.GetLivsForecast();
                    foreach (var item in result)
                    {
                        s += item.Key.ToString() + " " + item.Value.ToString() + "\n";
                    }
                    await Bot.SendTextMessageAsync(message.Chat.Id, s);
                    break;
                case "/exchange":
                    await Bot.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);
                    Exchange exchange = new Exchange();
                    String s2 = String.Empty;
                    List<string> result2 = exchange.GetCourse();
                    foreach (var item in result2)
                    {
                        s2 += item + "\n";
                    }
                    await Bot.SendTextMessageAsync(message.Chat.Id, s2);
                    break;
                default:
                    break;
            }
        }
        async private static void Roll(Message message)
        {
            await Bot.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);
            await Task.Delay(500);
            Random random = new Random();
            switch (random.Next(1, 10))
            {
                case 1:
                    await Bot.SendTextMessageAsync(
                            message.Chat.Id, message.From.FirstName + " очкосос!");
                    break;
                case 2:
                    await Bot.SendTextMessageAsync(
                            message.Chat.Id, message.From.FirstName + " йде нахуй!");
                    break;
                case 3:
                    await Bot.SendTextMessageAsync(
                            message.Chat.Id, message.From.FirstName + " сосе яйця!");
                    break;
                case 4:
                    await Bot.SendTextMessageAsync(
                            message.Chat.Id, message.From.FirstName + " пес!");
                    break;
                case 5:
                    await Bot.SendTextMessageAsync(
                            message.Chat.Id, message.From.FirstName + " ананіст!");
                    break;
                case 6:
                    await Bot.SendTextMessageAsync(
                            message.Chat.Id, message.From.FirstName + " козолуп!");
                    break;
                case 7:
                    await Bot.SendTextMessageAsync(
                            message.Chat.Id, message.From.FirstName + " делать коричневий грязь!");
                    break;
                case 8:
                    await Bot.SendTextMessageAsync(
                            message.Chat.Id, message.From.FirstName + " пєтух!");
                    break;
                default:
                    await Bot.SendTextMessageAsync(
                        message.Chat.Id, message.From.FirstName + " норм пацан!");
                    break;
            }
        }
    }
}
