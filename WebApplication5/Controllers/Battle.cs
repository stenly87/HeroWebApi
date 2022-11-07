using Microsoft.AspNetCore.Mvc;
using WebApplication5.Battle;
using WebApplication5.DB;
using WebApplication5.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Battle : ControllerBase
    {
        private readonly GameDBContext _context;
        private readonly BattlesMainLoop battleMainLoop;

        public Battle(GameDBContext context, BattlesMainLoop battleMainLoop)
        {
            _context = context;
            this.battleMainLoop = battleMainLoop;
        }

        // GET api/<Battle>/5
        [HttpGet("{id}")]
        public async Task<ActionResult> GetStatusRoom(int id)
        {
            var room = await _context.Rooms.FindAsync(id);
            if (room == null)
                return NotFound("room");

            BattleRoom battleRoom = battleMainLoop.GetBattleRoom(room);
            Hero[] heroes = battleRoom.GetHeroes();
            var turnHistory = _context.LogBattles.Where(s => s.IDRoom == id);
            int lastTurn = 0;
            if (turnHistory.Count() > 0)
                lastTurn = turnHistory.Max(s => s.Turn);
            var actions = new List<string>();
            if (lastTurn > 0)
                actions  = _context.LogBattles.Where(s => s.IDRoom == id && s.Turn == lastTurn).Select(s => s.HeroAction).ToList();
            return Ok(new BattleStatus { Heroes = heroes, Log = actions });
        }

        // PUT api/<Battle>/5
        [HttpPut("{id}")]
        public async Task<ActionResult> MakeAction(string guid, int action, int enemyId)
        {
            if (!Enum.GetValues<BattleActionType>().Contains((BattleActionType)action))
                return NotFound("action");

            if (battleMainLoop.HasActionInCurrentTurn(guid))
                return BadRequest();

            if (battleMainLoop.ContainHero(guid) == false)
                return NotFound("player");

            if (battleMainLoop.HasEnemy(guid, enemyId) == false)
                return NotFound("enemy");

            battleMainLoop.MakeAction(guid, action, enemyId);
            return Ok();
        }

    }
}
