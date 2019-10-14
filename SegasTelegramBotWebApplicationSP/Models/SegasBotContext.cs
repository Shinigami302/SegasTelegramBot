using Microsoft.EntityFrameworkCore;

namespace SegasTelegramBotWebApplicationSP.Models
{

    public class SegasBotContext : DbContext
    {
        public SegasBotContext(DbContextOptions<SegasBotContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<SBCommands> SBCommands { get; set; }
    }
}
