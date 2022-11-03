namespace WebApplication5.Models
{
    public class Room
    {
        public int ID { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public int CurrentTurn { get; set; }
        public bool IsActive { get; set; }
    }
}