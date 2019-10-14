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

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(SBCommands).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
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
