using System.Linq;
using EloBuddy;
using EloBuddy.SDK;

using Settings = KickassSeries.Champions.DrMundo.Config.Modes.LaneClear;

namespace KickassSeries.Champions.DrMundo.Modes
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

            if (Settings.UseQ && Q.IsReady() && jgminion.IsValidTarget(Q.Range))
            {
                Q.Cast(jgminion);
            }
            if (Settings.UseW && W.IsReady() && jgminion.IsValidTarget(W.Range) &&
                !Player.Instance.HasBuff("burningagony"))
            {
                {
                    W.Cast();
                }
            }
            if (Settings.UseE && E.IsReady() && jgminion.IsValidTarget(E.Range))
            {
                {
                    E.Cast();
                }
            }
        }
    }
}
