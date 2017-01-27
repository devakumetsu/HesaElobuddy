using System.Linq;
using EloBuddy;
using EloBuddy.SDK;

using Settings = KickassSeries.Champions.Ezreal.Config.Modes.LaneClear;

namespace KickassSeries.Champions.Ezreal.Modes
{
    public sealed class JungleClear : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear);
        }

        public override void Execute()
        {
            var jungleMonsters =
                EntityManager.MinionsAndMonsters.GetJungleMonsters()
                    .OrderByDescending(m => m.MaxHealth)
                    .FirstOrDefault(m => m.IsValidTarget(Q.Range));
            if (jungleMonsters == null) return;
            if (Settings.UseQ && Q.IsReady() && Settings.LaneMana <= Player.Instance.ManaPercent)
            {
                Q.Cast(jungleMonsters);
            }
        }
    }
}
