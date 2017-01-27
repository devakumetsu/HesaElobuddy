using System.Linq;
using EloBuddy;
using EloBuddy.SDK;

using Settings = KickassSeries.Champions.Ezreal.Config.Modes.LaneClear;

namespace KickassSeries.Champions.Ezreal.Modes
{
    public sealed class LaneClear : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear);
        }

        public override void Execute()
        {
            
            var laneMinion =
                EntityManager.MinionsAndMonsters.GetLaneMinions()
                    .OrderByDescending(m => m.Health)
                    .FirstOrDefault(
                        m => m.IsValidTarget(Q.Range) && m.Health <= SpellDamage.GetRealDamage(SpellSlot.Q, m));

            if (laneMinion == null) return;

            if (Settings.UseQ && Q.IsReady() && Settings.LaneMana <= Player.Instance.ManaPercent)
            {
                Q.Cast(laneMinion);
            }
            
        }
    }
}
