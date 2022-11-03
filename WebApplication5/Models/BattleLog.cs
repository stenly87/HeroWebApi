using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication5.Models
{
    
    public class BattleLog 
    {
        public int ID { get; set; }
        public int IDRoom { get; set; }
        public int Turn { get; set; }
        public string HeroAction { get; set; }
    }
}
