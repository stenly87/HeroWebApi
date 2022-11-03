using WebApplication5.DB;
using WebApplication5.Models;

namespace WebApplication5.Battle
{
    public class BattlesMainLoop
    {
        static Thread mainLoopTimer;
        static Dictionary<int, BattleRoom> rooms = new Dictionary<int, BattleRoom>();
        static Dictionary<string, int> players = new Dictionary<string, int>();
        private readonly GameDBContext context;

        public BattlesMainLoop(GameDBContext _context)
        {
            context = _context;
        }

        internal void CreateRoom(Player player, Room room)
        {
            var battleRoom = new BattleRoom(room);
            rooms.Add(room.ID, battleRoom);

            battleRoom.AddEnemy(player);
            players.Add(player.GUID, room.ID);
        }

        internal void JoinRoom(Player player, Room room)
        {
            rooms[room.ID].AddEnemy(player);
            players.Add(player.GUID, room.ID);
        }

        internal bool HasActionInCurrentTurn(string guid)
        {
            return rooms[players[guid]].HasAction(guid);
        }

        internal bool ContainHero(string guid)
        {
            return players.ContainsKey(guid);
        }

        internal bool HasEnemy(string guid, int enemyId)
        {
            return rooms[players[guid]].GetEnemy(enemyId);
        }

        internal void MakeAction(string guid, int action, int enemyId)
        {
            rooms[players[guid]].AddAction(guid, action, enemyId);
        }

        internal bool PlayerAlreadyInRoom(Player player)
        {
            throw new NotImplementedException();
        }
    }
}