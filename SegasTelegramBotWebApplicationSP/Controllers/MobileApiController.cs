using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SegasTelegramBotWebApplicationSP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MobileApiController : ControllerBase
    {
        // GET: api/MobileApi
        [HttpGet]
        public IEnumerable<string> Get()
        { 
            string [] botStatus = new string[2];
            botStatus[0] = "Bot is Alive";
            if (BotHome.GetBotHomeInstance.BotReaction)
            {
                botStatus[1] = "Bot Reaction is TURNED ON";
            }
            else
            {
                botStatus[1] = "Bot Reaction is TURNED OFF";
            }
            return botStatus;
        }

        // GET: api/MobileApi/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/MobileApi
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/MobileApi/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
