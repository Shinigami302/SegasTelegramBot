using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SegasTelegramBotWebApplicationSP.Models;

namespace SegasTelegramBotWebApplicationSP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SBController : ControllerBase
    {
        private readonly SegasBotContext _context;

        public SBController(SegasBotContext context)
        {
            _context = context;

            //if (_context.SBCommands.Count() == 0)
            //{
            //    _context.SBCommands.Add(new SBCommands { Answers = "Item1" });
            //    _context.SaveChanges();
            //}
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SBCommands>>> GetSBCommandItems()
        {
            return await _context.SBCommands.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SBCommands>> GetSBCommandItem(long id)
        {
            var commandsItem = await _context.SBCommands.FindAsync(id);

            if (commandsItem == null)
            {
                return NotFound();
            }

            return commandsItem;
        }
    }
}
