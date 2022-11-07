using System.Text;
using WebApplication5.Models;

namespace WebApplication5.Battle
{
    internal class BattleHelper
    {
        internal static string RunAction(Hero hero, BattleActionType action, Hero target)
        {
            switch (action)
            {
                case BattleActionType.AttackTarget:
                    return MakeAttack(hero, target);
            }
            return "error";
        }

        private static string MakeAttack(Hero hero, Hero target)
        {
            if (target.CurrentHP <= 0)
                return $"{hero.Name} попытался пнуть труп {target.Name}. Труп не отзывается.";
            Random rnd = new Random();
            int nDmg = rnd.Next(hero.Weapon.MinDamage, hero.Weapon.MaxDamage);
            int nNegHp = target.Armor.Resist - nDmg;
            bool blocked = false;
            if (nNegHp > 0)
                blocked = true;
            else
                target.CurrentHP += nNegHp;

            StringBuilder sb = new StringBuilder();
            sb.Append(hero.Name).Append(" схватил свой ");
            sb.Append(hero.Weapon.Name).Append(" и со всей силы ударил ");
            sb.Append(target.Name).Append(" прямо по голове. \n");
            if (blocked)
                sb.Append("Спасибо ").Append(target.Armor.Name).
                    Append(" - удар был полностью отражен");
            else
                sb.Append(target.Name).Append(" схватился за ушибленное место ").
                    Append(" и потерпел ").Append(nNegHp).Append(" урона. ").Append(target.Armor.Name).Append(" заблокировал ").Append(target.Armor.Resist).Append(" урона.");
            if (target.CurrentHP <= 0)
                sb.Append("\n").Append(target.Name).Append(" обиделся и умер");
            return sb.ToString();
        }
    }
}