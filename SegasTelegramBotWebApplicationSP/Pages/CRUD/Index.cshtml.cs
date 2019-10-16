using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SegasTelegramBotWebApplicationSP.Models;

namespace SegasTelegramBotWebApplicationSP.Pages.CRUD
{
    public class IndexModel : PageModel
    {
        private readonly SegasTelegramBotWebApplicationSP.Models.SegasBotContext _context;

        public IndexModel(SegasTelegramBotWebApplicationSP.Models.SegasBotContext context)
        {
            _context = context;
        }

        public IList<SBCommands> SBCommands { get; set; }
        public int ReactionChance { get; set; }
        public async Task OnGetAsync()
        {
            if (BotHome.GetBotHomeInstance.BotReaction)
            {
                ViewData["BotReaction"] = "Bot Reaction is TURNED ON";
            }
            else
            {
                ViewData["BotReaction"] = "Bot Reaction is TURNED OFF";
            }

            ViewData["BotReactionChance"] = BotHome.GetBotHomeInstance.ReactionChance.ToString();

            ViewData["Error"] = BotHome.GetBotHomeInstance.GetError;

            ReactionChance = BotHome.GetBotHomeInstance.ReactionChance;

            SBCommands = await _context.SBCommands.ToListAsync();
        }
    }
}
