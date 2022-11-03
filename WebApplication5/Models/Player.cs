namespace WebApplication5.Models
{
    public class Player
    {
        public int ID { get; set; }
        public string GUID { get; set; }
        public int HeroID { get; set; }
        public int RoomID { get; set; } = 1;

        public Hero Hero { get; set; }
        public Room Room { get; set; }
    }
}
