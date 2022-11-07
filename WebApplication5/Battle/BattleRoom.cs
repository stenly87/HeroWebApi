using WebApplication5.Models;

namespace WebApplication5.Battle
{
    internal class BattleRoom
    {
        private Room room;
        Dictionary<string, Hero> heroes = new Dictionary<string, Hero>();
        Dictionary<int, string> heroesId = new Dictionary<int, string>();
        Queue<BattleAction> actions = new Queue<BattleAction>();

        public bool IsActive { get => room.CurrentTurn > 0 && room.IsActive; }

        public BattleRoom(Room room)
        {
            this.room = room;
        }

        internal void AddEnemy(Player player)
        {
            heroes.Add(player.GUID, player.Hero);
            heroesId.Add(player.Hero.ID, player.GUID);
        }

        internal bool GetEnemy(int enemyId)
        {
            return heroesId.ContainsKey(enemyId);
        }

        internal void AddAction(string guid, int action, int targetID)
        {
            actions.Enqueue(new BattleAction { GUID = guid, Action = (BattleActionType)action, TargetID = targetID });
        }

        internal async Task MakeTurnAndStoreInfo(DB.GameDBContext context)
        {
            while(actions.Count > 0)
            {
                var action = actions.Dequeue();
                var hero = heroes[action.GUID];
                var target = heroes[heroesId[action.TargetID]];
                
                if (hero.CurrentHP > 0)
                {
                    string log = BattleHelper.RunAction(hero, action.Action, target);
                    context.LogBattles.Add(new BattleLog { IDRoom = room.ID, Turn = room.CurrentTurn, HeroAction = log });
                    context.Entry(hero).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    context.Entry(target).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                }
            }
            int aliveCount = heroes.Values.Count(s => s.CurrentHP > 0);
            if (aliveCount <= 1)
                room.IsActive = false;
            room.CurrentTurn++;
            context.Entry(room).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            await context.SaveChangesAsync();
        }

        internal bool HasAction(string guid)
        {
            return actions.FirstOrDefault(s => s.GUID == guid) != null;
        }
    }
}