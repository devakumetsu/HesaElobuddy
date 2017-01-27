using System.Linq;
using EloBuddy;
using EloBuddy.SDK;

namespace KickassSeries.Champions.Irelia.Modes
{
    public sealed class Flee : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Flee);
        }

        public override void Execute()
        {
            var minion = EntityManager.MinionsAndMonsters.Minions.OrderBy(x => x.Distance(Game.CursorPos)).FirstOrDefault(m => m.IsEnemy && m.IsValidTarget(Q.Range));

            if (minion != null)
            {
                Q.Cast(minion);
            }
        }
    }
}
