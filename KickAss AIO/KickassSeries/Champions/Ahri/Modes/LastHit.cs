using System.Linq;
using EloBuddy;
using EloBuddy.SDK;

using Settings = KickassSeries.Champions.Ahri.Config.Modes.LastHit;

namespace KickassSeries.Champions.Ahri.Modes
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

            //if (minion == null || Settings.ManaLast >= Player.Instance.ManaPercent) return;

            if (E.IsReady() && minion.IsValidTarget(E.Range) && Settings.UseE && Settings.LastMana < Player.Instance.ManaPercent)
            {
                var minionE =
                    EntityManager.MinionsAndMonsters.GetLaneMinions()
                        .OrderByDescending(m => m.Health)
                        .FirstOrDefault(
                            m => m.IsValidTarget(E.Range) && m.Health <= SpellDamage.GetRealDamage(SpellSlot.E, m));
                if (minionE != null)
                {
                    E.Cast(minion);
                }
            }

            if (Q.IsReady() && minion.IsValidTarget(Q.Range) && Settings.UseQ && Settings.LastMana < Player.Instance.ManaPercent)
            {
                var minionQ =
                    EntityManager.MinionsAndMonsters.GetLaneMinions()
                        .OrderByDescending(m => m.Health)
                        .FirstOrDefault(
                            m => m.IsValidTarget(E.Range) && m.Health <= SpellDamage.GetRealDamage(SpellSlot.Q, m));
                if (minionQ != null)
                {
                    Q.Cast(minion);
                }
            }
        }
    }
}
