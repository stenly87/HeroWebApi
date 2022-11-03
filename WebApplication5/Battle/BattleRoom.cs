using WebApplication5.Models;

namespace WebApplication5.Battle
{
    internal class BattleRoom
    {
        private Room room;
        Dictionary<string, Hero> heroes = new Dictionary<string, Hero>();
        Dictionary<int, string> heroesId = new Dictionary<int, string>();

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

        internal void AddAction(string guid, int action, int enemyId)
        {
            
        }

        internal bool HasAction(string guid)
        {
            return false;
        }
    }
}