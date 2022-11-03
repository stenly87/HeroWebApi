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
    public class ArmorsController : ControllerBase
    {
        private readonly GameDBContext _context;

        public ArmorsController(GameDBContext context)
        {
            _context = context;
        }

        // GET: api/Armors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Armor>>> GetArmors()
        {
            return await _context.Armors.ToListAsync();
        }

        // GET: api/Armors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Armor>> GetArmor(int id)
        {
            var armor = await _context.Armors.FindAsync(id);

            if (armor == null)
            {
                return NotFound();
            }

            return armor;
        }

        // PUT: api/Armors/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutArmor(int id, Armor armor)
        {
            if (id != armor.Id)
            {
                return BadRequest();
            }

            _context.Entry(armor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ArmorExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Armors
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Armor>> PostArmor(Armor armor)
        {
            _context.Armors.Add(armor);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetArmor", new { id = armor.Id }, armor);
        }

        // DELETE: api/Armors/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteArmor(int id)
        {
            var armor = await _context.Armors.FindAsync(id);
            if (armor == null)
            {
                return NotFound();
            }

            _context.Armors.Remove(armor);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ArmorExists(int id)
        {
            return _context.Armors.Any(e => e.Id == id);
        }
    }
}
