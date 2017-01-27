using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using SharpDX;
using Settings = KickassSeries.Champions.Alistar.Config.Modes.LaneClear;

namespace KickassSeries.Champions.Aatrox.Modes
{
    public sealed class LaneClear : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear);
        }

        public override void Execute()
        {
            var laneminion =
                EntityManager.MinionsAndMonsters.GetLaneMinions()
                    .OrderByDescending(m => m.Health)
                    .FirstOrDefault(m => m.IsValidTarget(Q.Range));

            if (laneminion == null) return;

            if (Q.IsReady() && laneminion.IsValidTarget(Q.Range) && Settings.UseQ)
            {

                Q.Cast(laneminion);
            }

            if (E.IsReady() && laneminion.IsValidTarget(E.Range) && Settings.UseE)
            {

                E.Cast(laneminion);
            }
        }
    }
}
