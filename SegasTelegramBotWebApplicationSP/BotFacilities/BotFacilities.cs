using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace SegasTelegramBotWebApplicationSP
{
    public static class BotFacilities
    {
        async public static void Roll(Message message, string range, TelegramBotClient bot)
        {
            int rangeInt;
            await bot.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);
            await Task.Delay(500);
            Random random = new Random();
            if (int.TryParse(range, out rangeInt))
            {
                await bot.SendTextMessageAsync(message.Chat.Id, $"{random.Next(1, rangeInt)}");
            }
            else
            {
                await bot.SendTextMessageAsync(message.Chat.Id, $"{random.Next(1, 100)}");
            }
        }
    }
}
