namespace SegasTelegramBotWebApplicationSP.Models
{
    public class SBCommands
    {
        public long Id { get; set; }
        public string ChatId { get; set; }
        public string ChatName { get; set; }
        public string Users { get; set; }
        public string Reactions { get; set; }
        public string HowAreYou { get; set; }
        public string Answers { get; set; }
        public string TriggersHowAreYou { get; set; }
        public string TriggersAnswers { get; set; }
        public string BotReaction { get; set; }
        public string ProverbOfTheDay { get; set; }
        public int ReactionChance { get; set; }
    }
}
