using System.Linq;
using EloBuddy.SDK;

using Settings = KickassSeries.Champions.Xerath.Config.Modes.LastHit;

namespace KickassSeries.Champions.Xerath.Modes
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

            if (minion.IsValidTarget(Q.MaximumRange) && Q.IsReady() && !Q.IsCharging && Settings.UseQ)
            {
                Q.StartCharging();
            }

            if (Q.IsCharging)
            {
                if (minion.IsValidTarget(Q.Range + 30))
                {
                    Q.Cast(minion);
                }
            }

            if (Q.IsCharging) return;

            if (E.IsReady() && minion.IsValidTarget(E.Range) && Settings.UseE)
            {
                E.Cast(minion);
            }

            if (W.IsReady() && minion.IsValidTarget(W.Range) && Settings.UseW)
            {
                W.Cast(minion);
            }
        }
    }
}
