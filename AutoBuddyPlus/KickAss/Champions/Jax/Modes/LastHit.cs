using System.Linq;
using EloBuddy;
using EloBuddy.SDK;

using Settings = KickassSeries.Champions.Jax.Config.Modes.LastHit;

namespace KickassSeries.Champions.Jax.Modes
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

            if (Q.IsReady() && minion.IsValidTarget(Q.Range) && Settings.UseQ)
            {
                var minionQ =
                    EntityManager.MinionsAndMonsters.GetLaneMinions()
                        .OrderByDescending(m => m.Health)
                        .FirstOrDefault(
                            m =>
                                m.IsValidTarget(Q.Range) && m.Health <= SpellDamage.GetRealDamage(SpellSlot.Q, m) &&
                                m.CountEnemiesInRange(400) >= 1);
                if (minionQ != null)
                {
                    Q.Cast(minion);
                }
            }

            if (EventsManager.CanW && minion.IsValidTarget(W.Range) && Settings.UseW)
            {
                var minionW =
                    EntityManager.MinionsAndMonsters.GetLaneMinions()
                        .OrderByDescending(m => m.Health)
                        .FirstOrDefault(
                            m =>
                                m.IsValidTarget(Q.Range) && m.Health <= SpellDamage.GetRealDamage(SpellSlot.W, m) + Player.Instance.GetAutoAttackDamage(m) &&
                                m.CountEnemiesInRange(400) >= 1);
                if (minionW != null)
                {
                    W.Cast();
                }
            }
        }
    }
}
