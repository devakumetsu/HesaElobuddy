using System.Linq;
using EloBuddy.SDK;

using Settings = KickassSeries.Champions.Yasuo.Config.Modes.LaneClear;

namespace KickassSeries.Champions.Yasuo.Modes
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

            if (Settings.UseQ && jgminion.IsValidTarget(Q.Range) && Q.IsReady())
            {
                Q.Cast(jgminion);
            }

            if (Settings.UseE && jgminion.IsValidTarget(E.Range) && E.IsReady())
            {
                E.Cast(jgminion);
            }
        }
    }
}
