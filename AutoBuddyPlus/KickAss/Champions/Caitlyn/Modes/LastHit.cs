using System.Linq;
using EloBuddy;
using EloBuddy.SDK;

using Settings = KickassSeries.Champions.Caitlyn.Config.Modes.LastHit;

namespace KickassSeries.Champions.Caitlyn.Modes
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
                    .FirstOrDefault(
                        m =>
                            m.IsValidTarget(Q.Range) && !m.IsInRange(Player.Instance, Player.Instance.AttackRange) &&
                            (m.Health <= SpellDamage.GetRealDamage(SpellSlot.Q, m)));
            if (minion == null) return;

            if (Q.IsReady() && Settings.UseQ)
            {
                Q.Cast(minion);
            }
        }
    }
}
