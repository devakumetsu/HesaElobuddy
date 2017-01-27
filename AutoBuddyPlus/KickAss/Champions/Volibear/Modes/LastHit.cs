using System.Linq;
using EloBuddy;
using EloBuddy.SDK;

using Settings = KickassSeries.Champions.Volibear.Config.Modes.LastHit;

namespace KickassSeries.Champions.Volibear.Modes
{
    public sealed class LastHit : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LastHit);
        }

        public override void Execute()
        {
            var minion =
                EntityManager.MinionsAndMonsters.GetLaneMinions()
                    .OrderByDescending(m => m.Health)
                    .FirstOrDefault(m => m.IsValidTarget(Q.Range));

            if (minion == null) return;

            if (E.IsReady() && minion.IsValidTarget(E.Range) && Settings.UseE && minion.Health <= SpellDamage.GetRealDamage(SpellSlot.E, minion))
            {
                E.Cast();
            }

            if (W.IsReady() && minion.IsValidTarget(W.Range) && Settings.UseW && minion.Health <= SpellDamage.GetRealDamage(SpellSlot.W, minion))
            {
                W.Cast(minion);
            }
        }
    }
}
