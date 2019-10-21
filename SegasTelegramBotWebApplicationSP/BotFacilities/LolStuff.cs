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
            string user = users[random.Next(0, 4)];

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
            string user = users[random.Next(0, 4)];

            await bot.SendTextMessageAsync(message.Chat.Id, "Дзвоню Аллаху...");
            await bot.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);
            await Task.Delay(1500);
            await bot.SendTextMessageAsync(message.Chat.Id, "Питаю, хто сьогодні красавчик...");
            await bot.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);
            await Task.Delay(1000);
            await bot.SendTextMessageAsync(message.Chat.Id, $"ГЕРОЙ ДНЯ: {user}!");
        }

        async public static void ProverbOfADay(Message message, TelegramBotClient bot)
        {
            await bot.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);
            await Task.Delay(500);
            string[] proverbs =
                {
                    "Спасибі вашому дому, йдіть на хуй." ,
                    "Зробив діло - пішов на хуй!" ,
                    "Баба з возу - пішла на хуй!" ,
                    "І вовки - на хуй, і вівці - на хуй!" ,
                    "Чим далі в ліс, тим ближче на хуй." ,
                    "Хотів як краще, а пішов на хуй." ,
                    "Дають - бери, а б'ють - йди на хуй." ,
                    "Дружба дружбою, а на хуй йди." ,
                    "Чия б корова мукала, а твоя йшла б на хуй." ,
                    "Навчання світло - а ти пішов на хуй." ,
                    "Сім разів відміряй, і йди на хуй." ,
                    "Скажи мені, хто твій друг і обидва - на хуй!" ,
                    "Чим би дитя не тішилося, а йшло би на хуй." ,
                    "Собака гавкає, а караван - йде на хуй." ,
                    "Прийшла біда - йди на хуй." ,
                    "Готуй сани влітку, а йди - на хуй." ,
                    "Зі своїм статутом - йди на хуй." ,
                    "Зустрічають по одягу, а проводжають на хуй." ,
                    "Слово - не горобець, вилетить - підеш на хуй." ,
                    "Від нашого столу - йдіть на хуй!" ,
                    "Якщо гора не йде до Магомеда, Магомед йде нахуй." ,
                    "Зробив  діло  - йди на хуй сміло." ,
                    "Риба шукає, де глибше, а ти - йдеш нахуй." ,
                    "Один в полі йде на хуй." ,
                    "На безриб'ї і рак йде на хуй." ,
                    "Непроханий гість - пішов на хуй." ,
                    "Краще хуй в руці, ніж пизда на горизонті!" ,
                    "Дві голови добре, а нахуй йти краще" ,
                    "Скільки вовка не годуй все рівно на хуй йде" ,
                    "Не май сто гривень, а нахуй йди" ,
                    "Сім раз відмір, один раз нахуй йди" ,
                    "Сильний переможе одного, знаючий нахуй йде" ,
                    "Той нe помиляється, хто нa хуй йде"
            };
            Random random = new Random();
            await bot.SendTextMessageAsync(message.Chat.Id, proverbs[random.Next(proverbs.Length)]);
        }

    }
}
