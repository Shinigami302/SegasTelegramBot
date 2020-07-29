using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace SegasTelegramBotWebApplicationSP
{ 
    public class ReactionSimulator
    {
        private DataReader _dataReader;
        private TelegramBotClient _bot;
        private int messageCountFromOneUser = 0;
        private User currentUser;

        public ReactionSimulator(DataReader dataReader, TelegramBotClient bot)
        {
            _dataReader = dataReader;
            _bot = bot;
        }

        public async void SimulateReaction(Message message, int reactionChance )
        {
            string messageText = message.Text;
            messageText = messageText.ToLower();
            Random random = new Random();
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
                    switch (random.Next(0, 3))
                    {
                        case 0:
                            await _bot.SendTextMessageAsync(message.Chat.Id, $"{message.From.FirstName}, воу-воу братан, полєгче. Все норм, розслабся.");
                            break;
                        case 1:
                            await _bot.SendTextMessageAsync(message.Chat.Id, $"{message.From.FirstName}, тихше, тихше. Все гуд.");
                            break;
                        case 2:
                            await _bot.SendTextMessageAsync(message.Chat.Id, $"{message.From.FirstName}, не розводь срач плз.");
                            break;
                    }
                }
            }

            int lenghtToCut = messageText.StartsWith("ботік") ? 5 : 3;
            if (messageText.StartsWith("бот") || messageText.StartsWith("ботік"))
            {
                messageText = messageText.Substring(lenghtToCut, messageText.Length - lenghtToCut);
                if (string.IsNullOrEmpty(messageText))
                {
                    await _bot.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);
                    await Task.Delay(500);
                    await _bot.SendTextMessageAsync(message.Chat.Id, "Чьо хочеш?");
                }
                string[] triggerAnswers = _dataReader.GetTriggersAnswers();
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
                    await _bot.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);
                    await Task.Delay(500);
                    string[] answers = _dataReader.GetAnswers();
                    string answer = answers[random.Next(0, answers.Length)];
                    await _bot.SendTextMessageAsync(message.Chat.Id, answer);
                    return;
                }

                string[] triggerHowAreYou = _dataReader.GetTriggersHowAreYou();
                bool triggerHowAreYouIsFound = false;
                foreach (string item in triggerHowAreYou)
                {
                    if (messageText.Contains(item))
                    {
                        triggerHowAreYouIsFound = true;
                        break;
                    }
                }
                if (triggerHowAreYouIsFound)
                {
                    await _bot.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);
                    await Task.Delay(500);
                    string[] howAreYouVar = _dataReader.GetHowAreYou();
                    string howAreYou = howAreYouVar[random.Next(0, howAreYouVar.Length)];
                    await _bot.SendTextMessageAsync(message.Chat.Id, howAreYou);
                    return;
                }
            }
            int randomNumber = random.Next(0, 101);

            if (reactionChance >= (randomNumber * 3))
            {
                ReactWithSticker(message, random.Next(0, 9));
            }
            else if (reactionChance >= randomNumber)
            {
                string[] reactions = _dataReader.GetReactions();
                string reaction = reactions[random.Next(1, reactions.Length)];
                if (reaction.Contains("{name}"))
                {
                    reaction = reaction.Replace("{name}", message.From.FirstName);
                }
                await _bot.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);
                await Task.Delay(500);
                await _bot.SendTextMessageAsync(message.Chat.Id, reaction);
            }
        }

        private async void ReactWithSticker(Message message, int stickerNumber)
        {
            String[] stickerPack = 
                {
                        "CAADAgADBgADanqJDVEvGLHrkPLJFgQ", "CAADAgADGAADanqJDYaPyedRFhhtFgQ",
                        "CAADAgADFgADanqJDe1Tx86_b2OYFgQ", "CAADAgADDQADanqJDeA6sTzcQDk4FgQ",
                        "CAADAgADFwADanqJDQPrIfd5UHdgFgQ", "CAADAgADEwADanqJDT3zKYyFbedPFgQ",
                        "CAADAgADFAADanqJDU3zomo8S2TvFgQ", "CAADAgADFAADanqJDU3zomo8S2TvFgQ"
                };
            await _bot.SendStickerAsync(message.Chat.Id, new Telegram.Bot.Types.InputFiles.InputOnlineFile(stickerPack[stickerNumber])); 
        }
    }
}
