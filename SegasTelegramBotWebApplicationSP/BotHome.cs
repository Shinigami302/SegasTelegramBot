using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;


namespace SegasTelegramBotWebApplicationSP
{
    internal class BotHome
    {
        private readonly string TELEGRAM_TOKEN = "962554948:AAHh6J2clB-gNm4vIemRgXk9NxdET4ZobG4";
        private TelegramBotClient bot;
        private static BotHome _instance;

        public static BotHome GetBotHomeInstance
        {
            get
            {
                if (null == _instance)
                {
                    _instance = new BotHome();
                }
                return _instance;
            }

        }

        private TelegramBotClient Bot
        {
            get
            {
                if (null == bot)
                {
                    bot = new TelegramBotClient(TELEGRAM_TOKEN) { Timeout = TimeSpan.FromSeconds(10) };
                }
                return bot;
            }
        }

        public void BotInit()
        {
            //var me = Bot.GetMeAsync().Result;
            Bot.OnMessage += BotOnMessageReceived;
            Bot.StartReceiving(Array.Empty<UpdateType>());
        }

        public void BotStopReceiving()
        {
            Bot.StopReceiving();
        }

        private async void BotOnMessageReceived(object sender, MessageEventArgs messageEventArgs)
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
                                    message.Chat.Id, @"Ну хулі тут, вибирай: 
                        /weather - погода у Львові 
                        /roll - рулетка
                        /exchange - курс валют
                        /homoOfADay - підар дня
                        /heroOfADay - герой дня");

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
                case "/homoOfADay":
                    await Bot.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);
                    HomoOfADay(message);
                    break;
                case "/heroOfADay":
                    await Bot.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);
                    HeroOfADay(message);
                    break;
                default:
                    break;
            }
        }
        async private void Roll(Message message)
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

        async private void HomoOfADay(Message message)
        {
            await Bot.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);
            await Task.Delay(500);
            string[] users = { "Сєрий", "Паша", "Бодя", "Вася", "Славік" };
            Random random = new Random();
            string user = users[random.Next(0, 4)];

            await Bot.SendTextMessageAsync(message.Chat.Id,"!!!КВЕРЯЮ БАЗУ ЖОВКІВСЬКОГО МВД!!!");
            await Bot.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);
            await Task.Delay(1000);
            await Bot.SendTextMessageAsync(message.Chat.Id, "Запрос сформовано і відправлено. Чекаю відповідь...");
            await Bot.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);
            await Task.Delay(1000);
            await Bot.SendTextMessageAsync(message.Chat.Id, "Інформацію отримано успішно");
            await Bot.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);
            await Task.Delay(1000);
            await Bot.SendTextMessageAsync(message.Chat.Id, $"ПІДАР СЬОГОДНІШНЬОГО ДНЯ: {user}!");
        }

        async private void HeroOfADay(Message message)
        {
            await Bot.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);
            await Task.Delay(500);
            string[] users = { "Сєрий", "Паша", "Бодя", "Вася", "Славік" };
            Random random = new Random();
            string user = users[random.Next(0, 4)];

            await Bot.SendTextMessageAsync(message.Chat.Id, "Дзвоню Аллаху...");
            await Bot.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);
            await Task.Delay(1500);
            await Bot.SendTextMessageAsync(message.Chat.Id, "Питаю, хто сьогодні красавчик...");
            await Bot.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);
            await Task.Delay(1000);
            await Bot.SendTextMessageAsync(message.Chat.Id, $"ГЕРОЙ ДНЯ: {user}!");

        }
    }
}
