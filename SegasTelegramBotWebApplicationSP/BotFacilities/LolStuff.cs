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
            await bot.SendTextMessageAsync(message.Chat.Id, "Потім гляну, а то зараз в базі пусто.");
        }

        async public static void HomoOfADay(Message message, TelegramBotClient bot, string[] users)
        {
            await bot.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);
            await Task.Delay(500);
            Random random = new Random();
            string user = users[random.Next(0, users.Length)];

            await bot.SendTextMessageAsync(message.Chat.Id, "Дзвоню вашим партнерам...");
            await bot.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);
            await Task.Delay(1000);
            await bot.SendTextMessageAsync(message.Chat.Id, "Чекаю відповідь...");
            await bot.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);
            await Task.Delay(1500);
            await bot.SendTextMessageAsync(message.Chat.Id, $"ГЕЙ ДНЯ: {user}!");
        }

        async public static void HeroOfADay(Message message, TelegramBotClient bot, string[] users)
        {
            await bot.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);
            await Task.Delay(500);
            Random random = new Random();
            string user = users[random.Next(0, users.Length)];

            await bot.SendTextMessageAsync(message.Chat.Id, "Пишу СМСку до Миколая...");
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
