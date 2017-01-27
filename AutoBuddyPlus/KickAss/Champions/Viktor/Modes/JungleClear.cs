using System.Linq;
using EloBuddy.SDK;

using Settings = KickassSeries.Champions.Viktor.Config.Modes.LaneClear;

namespace KickassSeries.Champions.Viktor.Modes
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
                    .FirstOrDefault(j => j.IsValidTarget(E.Range));

            if (jgminion == null)return;

            if (E.IsReady() && jgminion.IsValidTarget(E.Range) && Settings.UseE)
            {
                E.Cast(jgminion);
            }

            if (W.IsReady() && jgminion.IsValidTarget(W.Range) && Settings.UseW)
            {
                W.Cast(jgminion);
            }

            if (Q.IsReady() && jgminion.IsValidTarget(Q.Range) && Settings.UseQ)
            {
                Q.Cast();
            }
        }
    }
}
