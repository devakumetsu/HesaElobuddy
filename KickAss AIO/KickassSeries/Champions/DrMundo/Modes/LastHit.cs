using System.Linq;
using EloBuddy;
using EloBuddy.SDK;

using Settings = KickassSeries.Champions.DrMundo.Config.Modes.LastHit;

namespace KickassSeries.Champions.DrMundo.Modes
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

            if (Settings.UseQ && Q.IsReady())
            {
                var minionQ =
                    EntityManager.MinionsAndMonsters.GetLaneMinions()
                        .FirstOrDefault(
                            m =>
                                m.IsValidTarget(Q.Range) && m.IsEnemy &&
                                m.Health < SpellDamage.GetRealDamage(SpellSlot.Q, m));
                if (minionQ != null)
                {
                    Q.Cast(minionQ);
                }
            }
        }
    }
}
