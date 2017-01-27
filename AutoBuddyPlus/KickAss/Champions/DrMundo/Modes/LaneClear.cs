using System.Linq;
using EloBuddy;
using EloBuddy.SDK;

using Settings = KickassSeries.Champions.DrMundo.Config.Modes.LaneClear;

namespace KickassSeries.Champions.DrMundo.Modes
{
    public sealed class LaneClear : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear);
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
            if (Settings.UseW && W.IsReady() && minion.IsValidTarget(W.Range) &&
                !Player.Instance.HasBuff("burningagony"))
            {
                {
                    W.Cast();
                }
            }
            if (Settings.UseE && E.IsReady() && minion.IsValidTarget(E.Range))
            {
                {
                    E.Cast();
                }
            }
        }
    }
}
