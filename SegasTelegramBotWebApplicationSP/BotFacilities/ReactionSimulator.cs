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

        public ReactionSimulator(DataReader dataReader, TelegramBotClient bot)
        {
            _dataReader = dataReader;
            _bot = bot;
        }

        public async void SimulateReaction(Message message, User currentUser, int reactionChance )
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
                    await _bot.SendTextMessageAsync(message.Chat.Id, $"{message.From.FirstName}, воу-воу братан, полєгче. Все норм, розслабся.");
                }
            }
            Random random = new Random();

            int lenghtToCut = messageText.StartsWith("ботік") ? 5 : 3;
            if (messageText.StartsWith("бот") || messageText.StartsWith("ботік"))
            {
                messageText = messageText.Substring(lenghtToCut, messageText.Length - lenghtToCut);
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
                    string answer = answers[random.Next(1, answers.Length)];
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
                    string howAreYou = howAreYouVar[random.Next(1, howAreYouVar.Length)];
                    await _bot.SendTextMessageAsync(message.Chat.Id, howAreYou);
                    return;
                }
            }

            if ( reactionChance >= random.Next(1, 101))
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
    }
}
