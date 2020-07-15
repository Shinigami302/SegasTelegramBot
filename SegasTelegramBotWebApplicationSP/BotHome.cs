using Microsoft.EntityFrameworkCore;
using SegasTelegramBotWebApplicationSP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly string BOT_VERSION = "v0.9.150720";
        private TelegramBotClient bot;
        private static BotHome _instance;
        private bool botIsRunning;
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

        public string BotVersion
        {
            get 
            {
                return BOT_VERSION;
            }
        }

        public bool BotReaction 
        {
            get 
            { 
                return DataReader.GetBotReaction(); 
            }
            set 
            {
                try
                {
                    var result = _context.SBCommands.SingleOrDefault(r => r.ChatId == "1");
                    if (result != null)
                    {
                        result.BotReaction = value ? "TRUE" : "FALSE";
                        _context.SaveChanges();
                        DataReader.UpdateBotReactionBool(value);
                    }
                }
                catch (Exception ex)
                {
                    Error += ex.Message;
                }
            } 
        }

        public string Error { get; private set; }

        public int ReactionChance 
        { 
            get 
            {
                reactionChance = DataReader.GetReactionChance();
                if (reactionChance > 0)
                {
                    return reactionChance;
                }
                else
                {
                    return 7;
                }
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

        public ReactionSimulator ReactionSimulator { get; set; }

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
                //reactionChance = 0;
                //BotReaction = false;
                ReactionSimulator = new ReactionSimulator(DataReader, Bot);
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

            if (message.Text.Contains("@SegasBot"))
            {
                messagesText = message.Text.Substring(0, message.Text.IndexOf("@SegasBot"));
            }
            else
            {
                messagesText = message.Text;
            }

            if (firstRevMode)
            {
                if (messagesText.Contains("/botClassic") || messagesText.Contains("/botclassic"))
                {
                    firstRevMode = false;
                    await Bot.SendTextMessageAsync(message.Chat.Id, $"Режим бота першої ревізії ВИКЛ");
                }
                else
                {
                    await Bot.SendTextMessageAsync(message.Chat.Id, $"{message.Text}? та пішов ти нахуй, {message.From.FirstName}!");
                }
                return;
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
                    BotCommand[] commands = await Bot.GetMyCommandsAsync();
                    String commandsString = String.Empty;
                    foreach (var item in commands)
                    {
                        commandsString += $"/{item.Command} - {item.Description}\n";
                    }
                    await Bot.SendTextMessageAsync(
                        message.Chat.Id, "Ну чьо тут, вибирай: \n\n"
                        + commandsString);

                    // NEW COMMAND ADDING
                    //------------------------------------------------------------

                    //BotCommand[] commandsNew = new BotCommand[commands.Length + 1];
                    //for (int i = 0; i < commands.Length; i++)
                    //{
                    //    commandsNew[i] = commands[i];
                    //}
                    //BotCommand command = new BotCommand() { Command = "everybody", Description = "усі учасники чату" };
                    //commandsNew[commands.Length] = commandsNew[commands.Length - 1];
                    //commandsNew[commands.Length-1] = command;
                    //Bot.SetMyCommandsAsync(commandsNew);

                    //------------------------------------------------------------

                    break;
                case "/roll":
                    await Bot.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);
                    Roll(message, range);
                    break;
                case "/lolroll":
                    await Bot.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);
                    LolStuff.LolRoll(message, Bot);
                    break;
                case "/weather":
                    Forecast forecast = new Forecast();
                    String s = String.Empty;
                    try
                    {
                        Dictionary<string, string> result = forecast.GetLvivsForecast();
                        if (null != result)
                        {
                            foreach (var item in result)
                            {
                                s += item.Key.ToString() + " " + item.Value.ToString() + "\n";
                            }
                            await Bot.SendTextMessageAsync(message.Chat.Id, s);
                        }
                    }
                    catch (Exception ex)
                    {
                        Error += ex.Message;
                    }
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
                case "/homoofaday":
                    await Bot.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);
                    LolStuff.HomoOfADay(message, Bot, DataReader.GetUsers());
                    break;
                case "/heroofaday":
                    await Bot.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);
                    LolStuff.HeroOfADay(message, Bot, DataReader.GetUsers());
                    break;
                case "/proverbofaday":
                    await Bot.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);
                    LolStuff.ProverbOfADay(message, Bot, DataReader.GetProverbOfTheDay());
                    break;
                case "/botreaction":
                    if (BotReaction) await Bot.SendTextMessageAsync(message.Chat.Id, $"Реакція зараз включена.");
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
                case "/botclassic":
                    await Bot.SendTextMessageAsync(message.Chat.Id, $"Режим бота першої ревізії ВКЛ.");
                    firstRevMode = true;
                    break;
                case "/poll":
                    await Bot.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);
                    await Bot.SendPollAsync(message.Chat.Id, "Чьо мутимо на вихідних?", new List<String> { "Го до Паші", "Го центр",
                        "Го центр, потім до Паші", "Го за межі Жовкви", " Маю інші справи", "Не буду в Жовкві", "Я Паша. І мене не буде." });
                    break;
                case "/everybody":
                case "/g":
                    await Bot.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);
                    await Bot.SendTextMessageAsync(message.Chat.Id, "@Uncle_Serhii @pavlo_totskyi @silenthillph Бодя @Far1nHate ");
                    break;
                case "/info":
                    await Bot.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);
                    await Bot.SendTextMessageAsync(message.Chat.Id,
                        "---------------------------------------\n"
                        + "Admin page: shinigami302.somee.com/CRUD\n"
                        + "Mobile API: /api/MobileApi\n"
                        + $"FirstRevMode: {firstRevMode}\n"
                        + $"SegasTelegramBot {BotVersion}\n"
                        + "---------------------------------------\n");
                    break;
                default:
                    if (BotReaction) ReactionSimulator.SimulateReaction(message, ReactionChance);
                        break;
            }
        }

        private async void BotOnCallbackQueryReceived(object sender, CallbackQueryEventArgs callbackQueryEventArgs)
        {
            string answer = callbackQueryEventArgs.CallbackQuery.Data.ToString();

            switch (answer)
            {
                case "Вкл":
                    BotReaction = true;
                    await Bot.AnswerCallbackQueryAsync(callbackQueryEventArgs.CallbackQuery.Id, $"Реакція ботіка включена");
                    await Bot.SendTextMessageAsync(callbackQueryEventArgs.CallbackQuery.Message.Chat.Id, "Реакція ботіка включена");
                    break;
                case "Викл":
                    BotReaction = false;
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
            Error += receiveErrorEventArgs.ApiRequestException.Message;
        }

        async private void Roll(Message message, string range)
        {
            int rangeInt;
            await Bot.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);
            await Task.Delay(500);
            Random random = new Random();
            if(int.TryParse(range, out rangeInt))
            {
                await Bot.SendTextMessageAsync(message.Chat.Id, $"{random.Next(1, rangeInt)}");
            }
            else
            {
                await Bot.SendTextMessageAsync(message.Chat.Id, $"{random.Next(1, 100)}");
            }
        }
      
    }
}
