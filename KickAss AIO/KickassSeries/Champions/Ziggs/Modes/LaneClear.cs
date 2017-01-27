using System.Linq;
using EloBuddy.SDK;

using Settings = KickassSeries.Champions.Ziggs.Config.Modes.LaneClear;

namespace KickassSeries.Champions.Ziggs.Modes
{
    public sealed class LaneClear : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear);
        }

        public override void Execute()
        {
            var minion =
                EntityManager.MinionsAndMonsters.GetLaneMinions()
                    .OrderByDescending(m => m.Health)
                    .FirstOrDefault(m => m.IsValidTarget(Q3.Range));

            if (minion == null) return;

            if (Q1.IsReady() && minion.IsValidTarget(Q3.Range) && Settings.UseQ)
            {
                Functions.CastQ(minion);
            }

            if (E.IsReady() && minion.IsValidTarget(E.Range) && Settings.UseE)
            {
                E.Cast(minion);
            }

            if (W.IsReady() && minion.IsValidTarget(W.Range) && Settings.UseW)
            {
                W.Cast();
            }
        }
    }
}
