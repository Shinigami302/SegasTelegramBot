using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SegasTelegramBotWebApplicationSP.Models;

namespace SegasTelegramBotWebApplicationSP.Pages.CRUD
{
    public class EditModel : PageModel
    {
        private readonly SegasTelegramBotWebApplicationSP.Models.SegasBotContext _context;

        public EditModel(SegasTelegramBotWebApplicationSP.Models.SegasBotContext context)
        {
            _context = context;
        }

        [BindProperty]
        public SBCommands SBCommands { get; set; }
        [BindProperty]
        public int ReactionChance { get; set; }

        [BindProperty]
        public bool Reaction{ get; set; }

        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            ReactionChance = BotHome.GetBotHomeInstance.ReactionChance;
            SBCommands = await _context.SBCommands.FirstOrDefaultAsync(m => m.Id == id);
            Reaction = BotHome.GetBotHomeInstance.BotReaction;
            if (SBCommands == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            BotHome.GetBotHomeInstance.BotReaction = Reaction;
            BotHome.GetBotHomeInstance.ReactionChance = ReactionChance;
            _context.Attach(SBCommands).State = EntityState.Modified;

            try
            {
                int i = await _context.SaveChangesAsync();
                if (0 < i)
                {
                    BotHome.GetBotHomeInstance.ReInitDataReader(_context);
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SBCommandsExists(SBCommands.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool SBCommandsExists(long id)
        {
            return _context.SBCommands.Any(e => e.Id == id);
        }
    }
}
