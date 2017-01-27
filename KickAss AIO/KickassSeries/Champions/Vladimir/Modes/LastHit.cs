using System.Linq;
using EloBuddy;
using EloBuddy.SDK;

using Settings = KickassSeries.Champions.Vladimir.Config.Modes.LastHit;

namespace KickassSeries.Champions.Vladimir.Modes
{
    public sealed class LastHit : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LastHit);
        }

        public override void Execute()
        {
            var minion =
                EntityManager.MinionsAndMonsters.GetLaneMinions()
                    .OrderByDescending(m => m.Health)
                    .FirstOrDefault(m => m.IsValidTarget(Q.Range));

            if (minion == null) return;

            if (E.IsReady() && minion.IsValidTarget(E.Range) && Settings.UseE)
            {
                var minionE =
                       EntityManager.MinionsAndMonsters.GetLaneMinions()
                           .OrderByDescending(m => m.Health)
                           .FirstOrDefault(
                               m => m.IsValidTarget(E.Range) && m.Health <= SpellDamage.GetRealDamage(SpellSlot.E, m));

                if (minionE != null)
                {
                    E.Cast(minion);
                }
            }

            if (Q.IsReady() && minion.IsValidTarget(Q.Range) && Settings.UseQ)
            {
                var minionQ =
                       EntityManager.MinionsAndMonsters.GetLaneMinions()
                           .OrderByDescending(m => m.Health)
                           .FirstOrDefault(
                               m => m.IsValidTarget(Q.Range) && m.Health <= SpellDamage.GetRealDamage(SpellSlot.Q, m));

                if (minionQ != null)
                {
                    Q.Cast(minion);
                }
            }
        }
    }
}
