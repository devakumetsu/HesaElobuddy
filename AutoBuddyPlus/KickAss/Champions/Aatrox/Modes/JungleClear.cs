using System.Linq;
using EloBuddy;
using EloBuddy.SDK;

using Settings = KickassSeries.Champions.Alistar.Config.Modes.LaneClear;

namespace KickassSeries.Champions.Aatrox.Modes
{
    public sealed class JungleClear : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear);
        }

        public override void Execute()
        {
            var jugminion =
                EntityManager.MinionsAndMonsters.GetJungleMonsters()
                    .OrderBy(m => m.MaxHealth)
                    .FirstOrDefault(m => m.IsValidTarget(Q.Range));

            if (Q.IsReady() && jugminion.IsValidTarget(Q.Range) && Settings.UseQ)
            {
                Q.Cast(jugminion);
            }

            if (E.IsReady() && jugminion.IsValidTarget(E.Range) && Settings.UseE)
            {
                E.Cast(jugminion);
            }
        }
    }
}
