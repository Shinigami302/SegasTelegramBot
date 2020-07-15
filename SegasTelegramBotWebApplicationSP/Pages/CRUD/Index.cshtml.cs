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

        public async Task OnGetAsync()
        {
            if (BotHome.GetBotHomeInstance.BotReaction)
            {
                ViewData["BotReaction"] = "ON";
            }
            else
            {
                ViewData["BotReaction"] = "OFF";
            }

            ViewData["BotVersion"] = BotHome.GetBotHomeInstance.BotVersion.ToString();
            ViewData["ReactionChance"] = BotHome.GetBotHomeInstance.ReactionChance.ToString();

            ViewData["Error"] = BotHome.GetBotHomeInstance.Error;

            SBCommands = await _context.SBCommands.ToListAsync();
        }

    }
}