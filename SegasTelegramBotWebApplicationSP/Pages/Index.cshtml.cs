using Microsoft.AspNetCore.Mvc.RazorPages;
using SegasTelegramBotWebApplicationSP;

namespace SegasTelegramBotWebApplicationSP.Pages
{
    public class IndexModel : PageModel
    {
        public void OnGet()
        {
            if (BotHome.GetBotHomeInstance.BotReaction)
            {
                ViewData["BotReaction"] = "Bot Reaction is TURNED ON";
            }
            else
            {
                ViewData["BotReaction"] = "Bot Reaction is TURNED OFF";
            }

            ViewData["Error"] = BotHome.GetBotHomeInstance.GetError;
        }
    }
}
