using System.Linq;
using EloBuddy.SDK;

using Settings = KickassSeries.Champions.XinZhao.Config.Modes.LaneClear;

namespace KickassSeries.Champions.XinZhao.Modes
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

            if (E.IsReady() && jgminion.IsValidTarget(E.Range) && Settings.UseE)
            {
                E.Cast(jgminion);
            }

            if (EventsManager.CanW && jgminion.IsValidTarget(W.Range) && Settings.UseW)
            {
                W.Cast();
            }

            if (EventsManager.CanQ && jgminion.IsValidTarget(Q.Range) && Settings.UseQ)
            {
                Q.Cast();
            }
        }
    }
}
