using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SegasTelegramBotWebApplicationSP.Models;

namespace SegasTelegramBotWebApplicationSP.Pages.CRUD
{
    public class CreateModel : PageModel
    {
        private readonly SegasTelegramBotWebApplicationSP.Models.SegasBotContext _context;

        public CreateModel(SegasTelegramBotWebApplicationSP.Models.SegasBotContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public SBCommands SBCommands { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.SBCommands.Add(SBCommands);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}