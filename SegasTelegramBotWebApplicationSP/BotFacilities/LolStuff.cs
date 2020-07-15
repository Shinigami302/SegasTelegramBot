using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace SegasTelegramBotWebApplicationSP
{
    public static class LolStuff
    {
        async public static void LolRoll(Message message, TelegramBotClient bot)
        {
            await bot.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);
            await Task.Delay(500);
            Random random = new Random();
            switch (random.Next(1, 11))
            {
                case 1:
                    await bot.SendTextMessageAsync(
                            message.Chat.Id, message.From.FirstName + " очкосос!");
                    break;
                case 2:
                    await bot.SendTextMessageAsync(
                            message.Chat.Id, message.From.FirstName + " йде нахуй!");
                    break;
                case 3:
                    await bot.SendTextMessageAsync(
                            message.Chat.Id, message.From.FirstName + " сосе яйця!");
                    break;
                case 4:
                    await bot.SendTextMessageAsync(
                            message.Chat.Id, message.From.FirstName + " пес!");
                    break;
                case 5:
                    await bot.SendTextMessageAsync(
                            message.Chat.Id, message.From.FirstName + " ананіст!");
                    break;
                case 6:
                    await bot.SendTextMessageAsync(
                            message.Chat.Id, message.From.FirstName + " козолуп!");
                    break;
                case 7:
                    await bot.SendTextMessageAsync(
                            message.Chat.Id, message.From.FirstName + " делать коричневий грязь!");
                    break;
                case 8:
                    await bot.SendTextMessageAsync(
                            message.Chat.Id, message.From.FirstName + " пєтух!");
                    break;
                default:
                    await bot.SendTextMessageAsync(
                        message.Chat.Id, message.From.FirstName + " норм пацан!");
                    break;
            }
        }

        async public static void HomoOfADay(Message message, TelegramBotClient bot, string[] users)
        {
            await bot.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);
            await Task.Delay(500);
            Random random = new Random();
            string user = users[random.Next(0, users.Length)];

            await bot.SendTextMessageAsync(message.Chat.Id, "!!!КВЕРЯЮ БАЗУ ЖОВКІВСЬКОГО МВД!!!");
            await bot.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);
            await Task.Delay(1000);
            await bot.SendTextMessageAsync(message.Chat.Id, "Запрос сформовано і відправлено. Чекаю відповідь...");
            await bot.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);
            await Task.Delay(1500);
            await bot.SendTextMessageAsync(message.Chat.Id, $"ПІДАР ДНЯ: {user}!");
        }

        async public static void HeroOfADay(Message message, TelegramBotClient bot, string[] users)
        {
            await bot.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);
            await Task.Delay(500);
            Random random = new Random();
            string user = users[random.Next(0, users.Length)];

            await bot.SendTextMessageAsync(message.Chat.Id, "Дзвоню Аллаху...");
            await bot.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);
            await Task.Delay(1500);
            await bot.SendTextMessageAsync(message.Chat.Id, "Питаю, хто сьогодні красавчик...");
            await bot.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);
            await Task.Delay(1000);
            await bot.SendTextMessageAsync(message.Chat.Id, $"ГЕРОЙ ДНЯ: {user}!");
        }

        async public static void ProverbOfADay(Message message, TelegramBotClient bot, string[] proverbs)
        {
            await bot.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);
            await Task.Delay(500);
            
            Random random = new Random();
            await bot.SendTextMessageAsync(message.Chat.Id, proverbs[random.Next(proverbs.Length)]);
        }

    }
}
