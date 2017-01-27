using System.Linq;
using EloBuddy;
using EloBuddy.SDK;

using Settings = KickassSeries.Champions.Irelia.Config.Modes.LaneClear;

namespace KickassSeries.Champions.Irelia.Modes
{
    public sealed class LaneClear : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear);
        }

        public override void Execute()
        {
            if (Q.IsReady() && Settings.UseQ && Player.Instance.ManaPercent >= Settings.LaneMana)
            {
                var minionq =
                    EntityManager.MinionsAndMonsters.EnemyMinions.FirstOrDefault(
                        m => m.IsValidTarget(Q.Range) && m.Health <= SpellDamage.GetRealDamage(SpellSlot.Q, m));

                if (minionq != null)
                {
                    Q.Cast(minionq);
                }
            }

            if (EventsManager.CanW && Settings.UseW && Player.Instance.ManaPercent >= Settings.LaneMana)
            {
                var minionw =
                   EntityManager.MinionsAndMonsters.EnemyMinions.FirstOrDefault(
                       m => m.IsValidTarget(Player.Instance.GetAutoAttackRange()));

                if (minionw != null)
                {
                    W.Cast();
                }
            }
        }
    }
}
