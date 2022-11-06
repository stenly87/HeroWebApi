namespace WebApplication5.Battle
{
    public class BattleAction
    { 
        public string GUID { get; set; }
        public BattleActionType Action { get; set; }
        public int TargetID { get; set; }
    }
}
