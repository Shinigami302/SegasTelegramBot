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
    public class DeleteModel : PageModel
    {
        private readonly SegasTelegramBotWebApplicationSP.Models.SegasBotContext _context;

        public DeleteModel(SegasTelegramBotWebApplicationSP.Models.SegasBotContext context)
        {
            _context = context;
        }

        [BindProperty]
        public SBCommands SBCommands { get; set; }

        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            SBCommands = await _context.SBCommands.FirstOrDefaultAsync(m => m.Id == id);

            if (SBCommands == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            SBCommands = await _context.SBCommands.FindAsync(id);

            if (SBCommands != null)
            {
                _context.SBCommands.Remove(SBCommands);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
