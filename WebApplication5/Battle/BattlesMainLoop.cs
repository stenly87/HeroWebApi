using Microsoft.EntityFrameworkCore;
using WebApplication5.DB;
using WebApplication5.Models;

namespace WebApplication5.Battle
{
    public class BattlesMainLoop
    {
        const int durationTimer = 2500;
        static Dictionary<int, BattleRoom> rooms = new Dictionary<int, BattleRoom>();
        static Dictionary<string, int> players = new Dictionary<string, int>();
        private readonly GameDBContext context;

        public BattlesMainLoop(GameDBContext _context)
        {
            context = _context;
            var timer = async () => await CustomTimer();
            timer.Invoke();

            var players = context.Players.Include("Hero").ToList();
            var dbRooms = context.Rooms.ToList();
            players.ForEach(s => {
                if (s.RoomID != 0)
                {
                    if (rooms.ContainsKey(s.RoomID))
                        rooms[s.RoomID].AddEnemy(s);
                    else
                    {
                        var bdRoom = dbRooms.FirstOrDefault(r => r.ID == s.RoomID);
                        if (bdRoom != null)
                        {
                            var battleRoom = new BattleRoom(bdRoom);
                            battleRoom.AddEnemy(s);
                            rooms.Add(s.RoomID, battleRoom);
                        }
                        else
                        {
                            s.RoomID = 0;
                            var hero = context.Heroes.Find(s.HeroID);
                            if (hero != null)
                                hero.IsFree = true;
                        }
                    }
                }
            });
            foreach (var room in dbRooms)
            {
                if (!rooms.ContainsKey(room.ID))
                    rooms.Add(room.ID, new(room));
            }
            context.SaveChanges();
        }

        private async Task CustomTimer()
        {
            do
            {
                await Task.Delay(durationTimer);
                Console.WriteLine("battle turn");
                foreach (var room in rooms.Values)
                    if (room.IsActive)
                        await room.MakeTurnAndStoreInfo(context);
            }
            while (true);
        }

        internal BattleRoom GetBattleRoom(Room room)
        {
            return rooms[room.ID];
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
            return players.ContainsKey(player.GUID);
        }
    }
}