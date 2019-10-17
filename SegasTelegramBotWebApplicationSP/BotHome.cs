using Microsoft.EntityFrameworkCore;
using SegasTelegramBotWebApplicationSP.Models;
using System;
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
        private bool _reaction;
        private User currentUser;
        private int messageCountFromOneUser;
        private string error;
        private bool firstRevMode;
        private SegasBotContext _context;
        private DataReader _dataReader;
        private int reactionChance;

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
            get => _reaction;
            set { _reaction = value; }
        }

        public string GetError
        {
            get => error;
        }

        public int ReactionChance 
        { 
            get 
            {
                if (reactionChance > 0)
                {
                    return reactionChance;
                }
                else
                {
                    return 7;
                }
            } 
            set 
            { 
                reactionChance = value; 
            } 
        }

        private DataReader DataReader
        {
            get
            {
                if (null == _dataReader && null != _context)
                {
                    _dataReader = new DataReader(_context);
                    return _dataReader;
                }
                return _dataReader; 
            }
 
        }

        public void ReInitDataReader(SegasBotContext context)
        {
            DataReader.ReInit(context);
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

        public void BotInit(SegasBotContext context)
        {
            //var me = Bot.GetMeAsync().Result;
            if (!botIsRunning)
            {
                botIsRunning = true;
                _context = context;
                Bot.OnMessage += BotOnMessageReceived;
                Bot.OnMessageEdited += BotOnMessageReceived;
                Bot.OnCallbackQuery += BotOnCallbackQueryReceived;
                Bot.OnReceiveError += BotOnReceiveError;
                Bot.StartReceiving(Array.Empty<UpdateType>());
                messageCountFromOneUser = 0;
                reactionChance = 0;
                currentUser = null;
                _reaction = false;
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
            if (message == null || message.Type != MessageType.Text) return;
            if (firstRevMode)
            {
                if (!message.Text.Contains("/botClassic"))
                {
                    await Bot.SendTextMessageAsync(message.Chat.Id, $"{message.Text}? да пішов ти нахуй, {message.From.FirstName}!");
                }
                else
                {
                    firstRevMode = false;
                    await Bot.SendTextMessageAsync(message.Chat.Id, $"Режим бота першої ревізії ВИКЛ");
                }
                return;
            }
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
                        + "/heroOfADay - герой дня\n");

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
                    if (_reaction) await Bot.SendTextMessageAsync(message.Chat.Id, $"Реакція зараз включена.");
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
                case "/botClassic":
                    await Bot.SendTextMessageAsync(message.Chat.Id, $"Режим бота першої ревізії ВКЛ.");
                    firstRevMode = true;
                    break;
                default:
                    if (_reaction) SimulateReaction(message);
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
            
            int lenghtToCut = messageText.StartsWith("ботік") ? 5 : 3;
            if (messageText.StartsWith("бот") || messageText.StartsWith("ботік"))
            {
                messageText = messageText.Substring(lenghtToCut, messageText.Length - lenghtToCut);
                string[] triggerAnswers = DataReader.GetTriggersAnswers();
                bool triggerAnswerIsfound = false;
                foreach (string item in triggerAnswers)
                {
                    if (messageText.Contains(item))
                    {
                        triggerAnswerIsfound = true;
                        break;
                    }
                }
                if (triggerAnswerIsfound)
                {
                    await Bot.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);
                    await Task.Delay(500);
                    string[] answers = DataReader.GetAnswers();
                    string answer = answers[random.Next(1, answers.Length)];
                    await Bot.SendTextMessageAsync(message.Chat.Id, answer);
                    return;
                }

                string[] triggerHowAreYou= DataReader.GetTriggersHowAreYou();
                bool triggerHowAreYouIsfound = false;
                foreach (string item in triggerHowAreYou)
                {
                    if (messageText.Contains(item))
                    {
                        triggerHowAreYouIsfound = true;
                        break;
                    }
                }
                if (triggerHowAreYouIsfound)
                {
                    await Bot.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);
                    await Task.Delay(500);
                    string[] howAreYouVar = DataReader.GetHowAreYou();
                    string howAreYou = howAreYouVar[random.Next(1, howAreYouVar.Length)];
                    await Bot.SendTextMessageAsync(message.Chat.Id, howAreYou);
                    return;
                }
            }

            if (ReactionChance >= random.Next(1, 101))
            {
                string[] reactions = DataReader.GetReactions();
                string reaction = reactions[random.Next(1, reactions.Length)];
                if (reaction.Contains("{name}"))
                {
                    reaction = reaction.Replace("{name}", message.From.FirstName);
                }
                await Bot.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);
                await Task.Delay(500);
                await Bot.SendTextMessageAsync(message.Chat.Id, reaction);
            }           
        }

        private async void BotOnCallbackQueryReceived(object sender, CallbackQueryEventArgs callbackQueryEventArgs)
        {
            string answer = callbackQueryEventArgs.CallbackQuery.Data.ToString();

            switch (answer)
            {
                case "Вкл":
                    _reaction = true;
                    await Bot.AnswerCallbackQueryAsync(callbackQueryEventArgs.CallbackQuery.Id, $"Реакція ботіка включена");
                    await Bot.SendTextMessageAsync(callbackQueryEventArgs.CallbackQuery.Message.Chat.Id, "Реакція ботіка включена");
                    break;
                case "Викл":
                    _reaction = false;
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
            string[] users = DataReader.GetUsers();
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
            string[] users = DataReader.GetUsers();
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
