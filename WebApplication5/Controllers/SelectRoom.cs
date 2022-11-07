using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication5.Battle;
using WebApplication5.DB;
using WebApplication5.Models;

namespace WebApplication5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SelectRoom : ControllerBase
    {
        private readonly GameDBContext _context;
        private readonly BattlesMainLoop battleMainLoop;

        public SelectRoom(GameDBContext context, BattlesMainLoop battleMainLoop)
        {
            _context = context;
            this.battleMainLoop = battleMainLoop;
        }

        // GET: api/SelectRoom
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Room>>> GetRooms()
        {
            return await _context.Rooms.Where(s=>s.ID != 1).ToListAsync();
        }

        // PUT: api/SelectRoom/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRoom(int id, string guid)
        {
            if (id == 1)
                return BadRequest();

            var player = await _context.Players.Include("Hero").FirstOrDefaultAsync(s => s.GUID == guid);
            if (player == null)
                return NotFound("player");

            var room = await _context.Rooms.FindAsync(id);
            if (room == null)
                return NotFound("room");

            room.IsActive = true;
            if (room.CurrentTurn == 0)
                room.CurrentTurn++;

            if (battleMainLoop.PlayerAlreadyInRoom(player))
                return BadRequest();

            player.RoomID = room.ID;
            await _context.SaveChangesAsync();
            battleMainLoop.JoinRoom(player, room, _context);

            return NoContent();
        }

        // POST: api/SelectRoom
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<int>> PostRoom(string guid)
        {
            var player = await _context.Players.FirstOrDefaultAsync(s => s.GUID == guid);
            if (player == null)
                return NotFound();

            var room = new Room { IsActive = true };
            _context.Rooms.Add(room);
            await _context.SaveChangesAsync();

            player.RoomID = room.ID;
            await _context.SaveChangesAsync();

            battleMainLoop.CreateRoom(player, room);

            return room.ID;
        }

    }
}
