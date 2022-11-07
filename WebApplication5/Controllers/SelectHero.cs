using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication5.DB;
using WebApplication5.Models;

namespace WebApplication5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SelectHero : ControllerBase
    {
        private readonly GameDBContext _context;

        public SelectHero(GameDBContext context)
        {
            _context = context;
            
            Console.WriteLine("say hello");
        }

        // GET: api/SelectHero
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Hero>>> GetHeroes()
        {
            return await _context.Heroes.Where(s=>s.IsFree).ToListAsync();
        }

        // POST: api/SelectHero
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<string>> PostHero(int idHero)
        {
            var hero = await _context.Heroes.Include("Weapon").Include("Armor").FirstOrDefaultAsync(s=>s.ID == idHero);
            if (hero == null)  
                return NotFound();

            hero.IsFree = false;

            var player = new Player
            {
                GUID = Guid.NewGuid().ToString(),
                HeroID = idHero,
                RoomID = 1
            };

            await _context.Players.AddAsync(player);
            await _context.SaveChangesAsync();

            return player.GUID;
        }
    }
}
