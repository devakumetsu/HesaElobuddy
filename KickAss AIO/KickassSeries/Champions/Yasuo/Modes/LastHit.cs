using System.Linq;
using EloBuddy;
using EloBuddy.SDK;

using Settings = KickassSeries.Champions.Yasuo.Config.Modes.LastHit;

namespace KickassSeries.Champions.Yasuo.Modes
{
    public sealed class LastHit : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LastHit);
        }

        public override void Execute()
        {
            if (Settings.UseE && SpellManager.E.IsReady())
            {
                var minionE =
                    EntityManager.MinionsAndMonsters.EnemyMinions
                        .FirstOrDefault(m => m.IsEnemy && m.IsValidTarget(SpellManager.E.Range)
                                             && (m.Health <= SpellDamage.EDamage(m)));

                if (minionE != null && !minionE.GetAfterEPos().Tower())
                {
                    SpellManager.E.Cast(minionE);
                }
            }

            if (Settings.UseQ && SpellManager.Q.IsReady() && !Player.Instance.HasQ3())
            {
                var minionQ =
                    EntityManager.MinionsAndMonsters.EnemyMinions
                        .FirstOrDefault(m => m.IsEnemy && m.IsValidTarget(SpellManager.Q.Range)
                                             && m.Health <= SpellDamage.QDamage(m));
                if (minionQ != null)
                {
                    SpellManager.Q.Cast(minionQ);
                }

            }

            if (/*Settings.UseQ3 &&*/ Player.Instance.HasQ3() && SpellManager.Q.IsReady())
            {
                var minionQ3 =
                    EntityManager.MinionsAndMonsters.EnemyMinions
                        .FirstOrDefault(m => m.IsEnemy && m.IsValidTarget(SpellManager.Q.Range)
                                             && m.Health <= SpellDamage.QDamage(m));
                if (minionQ3 != null)
                {
                    SpellManager.Q.Cast(minionQ3);
                }

            }
        }
    }
}
