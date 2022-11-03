namespace WebApplication5.Models
{
    public class Hero
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int MaxHP { get; set; }
        public int CurrentHP { get; set; }

        public int ArmorID { get; set; }
        public int WeaponID { get; set; }

        public bool IsFree { get; set; } = true;

        public Armor Armor { get; set; }
        public Weapon Weapon { get; set; }
    }
}
