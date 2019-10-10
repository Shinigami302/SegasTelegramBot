﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace SegasTelegramBotWebApplicationSP
{
    internal class BotHome
    {
        private readonly string TELEGRAM_TOKEN = "962554948:AAHh6J2clB-gNm4vIemRgXk9NxdET4ZobG4";
        private TelegramBotClient bot;
        private static BotHome _instance;
        private bool botIsRunning;
        private bool reaction;
        private User currentUser;
        private int messageCountFromOneUser;
        private string error;

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

        public bool BotReaction
        {
            get => reaction;
        }

        public string GetError
        {
            get => error;
        }

        private TelegramBotClient Bot
        {
            get
            {
                if (null == bot)
                {
                    bot = new TelegramBotClient(TELEGRAM_TOKEN);
                }
                return bot;
            }
        }

        public void BotInit()
        {
            //var me = Bot.GetMeAsync().Result;
            if (!botIsRunning)
            {
                botIsRunning = true;
                Bot.OnMessage += BotOnMessageReceived;
                Bot.OnMessageEdited += BotOnMessageReceived;
                Bot.OnCallbackQuery += BotOnCallbackQueryReceived;
                Bot.OnReceiveError += BotOnReceiveError;
                Bot.StartReceiving(Array.Empty<UpdateType>());
                messageCountFromOneUser = 0;
                currentUser = null;
                reaction = false;
            }
        }

        public void BotStopReceiving()
        {
            Bot.StopReceiving();
        }

        private async void BotOnMessageReceived(object sender, MessageEventArgs messageEventArgs)
        {
            var message = messageEventArgs.Message;
            string messagesText = String.Empty;
            string range = String.Empty;
            //Console.WriteLine(message.From.FirstName + " : " + message.Text);
            if (message == null || message.Type != MessageType.Text) return;

            if (message.Text.Contains("@SegasBot"))
            {
                messagesText = message.Text.Substring(0, message.Text.IndexOf("@SegasBot"));
            }
            else
            {
                messagesText = message.Text;
            }

            if (messagesText.StartsWith("/roll"))
            {
                range = messagesText.Substring(5, messagesText.Length - 5);
                messagesText = "/roll";
            }

            switch (messagesText)
            {
                case "/start":
                    await Bot.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);
                    await Task.Delay(500);
                    await Bot.SendTextMessageAsync(
                                    message.Chat.Id, "Ну хулі тут, вибирай: \n\n"
                        + "/weather - погода у Львові\n" 
                        + "/roll - рулетка ( дефолт 1-100 )\n"
                        + "/lolRoll - LOL рулетка\n"
                        + "/exchange - курс валют\n"
                        + "/homoOfADay - підар дня\n"
                        + "/heroOfADay - герой дня\n"
                        + "/botReaction - реакція\n");

                    break;
                case "/roll":
                    await Bot.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);
                    Roll(message, range);
                    break;
                case "/lolRoll":
                    await Bot.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);
                    LolRoll(message);
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
                case "/botReaction":
                    if (reaction) await Bot.SendTextMessageAsync(message.Chat.Id, $"Реакція зараз включена.");
                    else await Bot.SendTextMessageAsync(message.Chat.Id, $"Реакція зараз виключена.");
                    var inlineKeyboard = new InlineKeyboardMarkup(new[]
                    {
                        new []
                        {
                            InlineKeyboardButton.WithCallbackData("Вкл"),
                            InlineKeyboardButton.WithCallbackData("Викл"),
                        },
                    });
                    await Bot.SendTextMessageAsync(message.Chat.Id, "Реакцію бота?", replyMarkup: inlineKeyboard);
                    break;
                default:
                    if (reaction) SimulateReaction(message);
                        break;
            }
        }

        private async void SimulateReaction(Message message)
        {
            string messageText = message.Text;
            messageText = messageText.ToLower();

            if (currentUser != message.From)
            {
                currentUser = message.From;
                messageCountFromOneUser = 0;
            }
            else
            {
                messageCountFromOneUser++;
                if (5 == messageCountFromOneUser)
                {
                    await Bot.SendTextMessageAsync(message.Chat.Id, $"{message.From.FirstName}, воу-воу братан, полєгче. Все норм, розслабся.");
                }
            }
            Random random = new Random();
            
            int lenghtToCut = messageText.StartsWith("бот") ? 3 : 5;
            if (messageText.StartsWith("бот") || messageText.StartsWith("ботік"))
            {
                messageText = messageText.Substring(lenghtToCut, messageText.Length - lenghtToCut);
                if (messageText.Contains("думаєш") || messageText.Contains("чьо скажеш"))
                {
                    await Bot.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);
                    await Task.Delay(500);
                    switch (random.Next(1, 4))
                    {
                        case 1:
                            await Bot.SendTextMessageAsync(message.Chat.Id, $"Думаю норм тема!");
                            break;
                        case 2:
                            await Bot.SendTextMessageAsync(message.Chat.Id, $"Якщо чесно, думаю то хуйня.");
                            break;
                        case 3:
                            await Bot.SendTextMessageAsync(message.Chat.Id, $"Ну... хз хз.");
                            break;
                    }
                }
                if (messageText.Contains("як справи") || messageText.Contains("чьо там"))
                {
                    await Bot.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);
                    await Task.Delay(500);
                    switch (random.Next(1, 4))
                    {
                        case 1:
                            await Bot.SendTextMessageAsync(message.Chat.Id, $"Нормас, живий поки і на тому дякую :)");
                            break;
                        case 2:
                            await Bot.SendTextMessageAsync(message.Chat.Id, $"Та щось так собі. Погода походу міняється.");
                            break;
                        case 3:
                            await Bot.SendTextMessageAsync(message.Chat.Id, $"Чесно, братан, хуйово. Біток знову падає...");
                            break;
                    }
                }
            }

            switch (random.Next(1, 100))
            {
                case 1:
                    await Bot.SendTextMessageAsync(message.Chat.Id, $"{message.From.FirstName}, ну ти знову за своє?");
                    break;
                case 2:
                    await Bot.SendTextMessageAsync(message.Chat.Id, $"{message.From.FirstName}, далі хуйню пишеш...");
                    break;
                case 3:
                    await Bot.SendTextMessageAsync(message.Chat.Id, $"Тру сторі, бро.");
                    break;
                case 4:
                    await Bot.SendTextMessageAsync(message.Chat.Id, $"Да да, апруваю!");
                    break;
                case 5:
                    await Bot.SendTextMessageAsync(message.Chat.Id, $"Буває.");
                    break;
                case 6:
                    await Bot.SendTextMessageAsync(message.Chat.Id, $"Йди посри, мб пройде.");
                    break;
                default:
                    break;
            }
        }

        private async void BotOnCallbackQueryReceived(object sender, CallbackQueryEventArgs callbackQueryEventArgs)
        {
            String answer = callbackQueryEventArgs.CallbackQuery.Data.ToString();
            switch (answer)
            {
                case "Вкл":
                    reaction = true;
                    await Bot.AnswerCallbackQueryAsync(callbackQueryEventArgs.CallbackQuery.Id, $"Реакція ботіка включена");
                    await Bot.SendTextMessageAsync(callbackQueryEventArgs.CallbackQuery.Message.Chat.Id, "Реакція ботіка включена");
                    break;
                case "Викл":
                    reaction = false;
                    await Bot.AnswerCallbackQueryAsync(callbackQueryEventArgs.CallbackQuery.Id, $"Реакція ботіка виключена");
                    await Bot.SendTextMessageAsync(callbackQueryEventArgs.CallbackQuery.Message.Chat.Id, "Реакція ботіка виключена");
                    break;
                default:
                    break;
            }
            await Task.Delay(1500);
            await Bot.DeleteMessageAsync(callbackQueryEventArgs.CallbackQuery.Message.Chat.Id, callbackQueryEventArgs.CallbackQuery.Message.MessageId);
        }

        private void BotOnReceiveError(object sender, ReceiveErrorEventArgs receiveErrorEventArgs)
        {
            error += receiveErrorEventArgs.ApiRequestException.Message;
        }

        async private void Roll(Message message, string range)
        {
            int rangeInt;
            await Bot.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);
            await Task.Delay(500);
            Random random = new Random();
            if(int.TryParse(range, out rangeInt))
            {
                await Bot.SendTextMessageAsync(message.Chat.Id, $"Випало: {random.Next(1, rangeInt)}");
            }
            else
            {
                await Bot.SendTextMessageAsync(message.Chat.Id, $"Випало: {random.Next(1, 100)}");
            }
        }
        async private void LolRoll(Message message)
        {
            await Bot.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);
            await Task.Delay(500);
            Random random = new Random();
            switch (random.Next(1, 11))
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
            await Task.Delay(1500);
            await Bot.SendTextMessageAsync(message.Chat.Id, $"ПІДАР ДНЯ: {user}!");
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
