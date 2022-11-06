using Microsoft.AspNetCore.Mvc;
using WebApplication5.Battle;
using WebApplication5.DB;

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
            return Ok();
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
