using WebApplication5.Models;

namespace WebApplication5.Battle
{
    internal class BattleStatus
    {
        public Hero[] Heroes { get; set; }
        public IQueryable<string> Log { get; set; }
    }
}