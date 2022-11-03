using Microsoft.EntityFrameworkCore;
using WebApplication5.Models;

namespace WebApplication5.DB
{
    public class GameDBContext : DbContext
    {
        public DbSet<Hero> Heroes { get; set; }
        public DbSet<Armor> Armors { get; set; }
        public DbSet<Weapon> Weapons { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<BattleLog> LogBattles { get; set; }
        

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("server=192.168.1.15\\sqlexpress;" +
                "database=1135_dbgame;user=student;password=student;");
            base.OnConfiguring(optionsBuilder);
        }

        public GameDBContext()
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
            
            if (Rooms.Count() == 0)
                Rooms.Add(new Room { IsActive = false });
            SaveChanges();
            
        }
    }
}
