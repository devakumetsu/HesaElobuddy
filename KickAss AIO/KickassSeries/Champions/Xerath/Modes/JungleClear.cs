using System.Linq;
using EloBuddy.SDK;

using Settings = KickassSeries.Champions.Xerath.Config.Modes.LaneClear;

namespace KickassSeries.Champions.Xerath.Modes
{
    public sealed class JungleClear : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear);
        }

        public override void Execute()
        {
            var jgminion =
                EntityManager.MinionsAndMonsters.GetJungleMonsters()
                    .OrderByDescending(j => j.Health)
                    .FirstOrDefault(j => j.IsValidTarget(Q.Range));

            if (jgminion == null)return;

            if (jgminion.IsValidTarget(Q.MaximumRange) && Q.IsReady() && !Q.IsCharging && Settings.UseQ)
            {
                Q.StartCharging();
            }

            if (Q.IsCharging)
            {
                if (jgminion.IsValidTarget(Q.Range + 30))
                {
                    Q.Cast(jgminion);
                }
            }

            if (Q.IsCharging) return;

            if (E.IsReady() && jgminion.IsValidTarget(E.Range) && Settings.UseE)
            {
                E.Cast(jgminion);
            }

            if (W.IsReady() && jgminion.IsValidTarget(W.Range) && Settings.UseW)
            {
                W.Cast(jgminion);
            }
        }
    }
}
