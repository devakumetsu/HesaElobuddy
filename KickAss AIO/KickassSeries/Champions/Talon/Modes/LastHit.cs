using System.Linq;
using EloBuddy;
using EloBuddy.SDK;

using Settings = KickassSeries.Champions.Talon.Config.Modes.LastHit;

namespace KickassSeries.Champions.Talon.Modes
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

            if (W.IsReady() && minion.IsValidTarget(W.Range) && Settings.UseW)
            {
                var minionW =
                    EntityManager.MinionsAndMonsters.GetLaneMinions()
                        .OrderByDescending(m => m.Health)
                        .FirstOrDefault(
                            m => m.IsValidTarget(W.Range) && m.Health <= SpellDamage.GetRealDamage(SpellSlot.W, m));
                if (minionW != null)
                {
                    W.Cast(minionW);
                }

            }
        }
    }
}
