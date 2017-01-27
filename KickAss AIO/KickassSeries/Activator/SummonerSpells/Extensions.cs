using System.Linq;
using EloBuddy;

namespace KickassSeries.Activator.SummonerSpells
{
    public class Extensions
    {
        public static bool HasSpell(string s)
        {
            return Player.Spells.FirstOrDefault(o => o.SData.Name.Contains(s)) != null;
        }
    }
}
