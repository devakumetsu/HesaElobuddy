using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;
using Settings = KickassSeries.Champions.Yasuo.Config.Modes.LaneClear;

namespace KickassSeries.Champions.Yasuo.Modes
{
    public sealed class LaneClear : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear);
        }

        public override void Execute()
        {
            if (Settings.UseQ && Settings.UseE && SpellManager.Q.IsReady() && SpellManager.E.IsReady())
            {
                var minionQe =
                    EntityManager.MinionsAndMonsters.EnemyMinions.OrderByDescending(m => m.Health)
                        .FirstOrDefault(
                            m =>
                                m.IsEnemy && m.IsValidTarget(SpellManager.Q.Range) && m.CanE() &&
                                (m.Health <= (SpellDamage.EDamage(m) + SpellDamage.QDamage(m))));
                if (minionQe != null)
                {
                    SpellManager.Q.Cast(minionQe);
                }
            }

            if (Settings.UseE && SpellManager.E.IsReady())
            {
                var minionE =
                    EntityManager.MinionsAndMonsters.EnemyMinions.OrderByDescending(m => m.Health)
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
                    EntityManager.MinionsAndMonsters.EnemyMinions.OrderByDescending(m => m.Health)
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
                    EntityManager.MinionsAndMonsters.EnemyMinions.OrderByDescending(m => m.Health)
                        .FirstOrDefault(m => m.IsEnemy && m.IsValidTarget(SpellManager.Q.Range)
                                             && m.Health <= SpellDamage.QDamage(m));
                if (minionQ3 != null)
                {
                    SpellManager.Q.Cast(minionQ3);
                }

            }

            if (Player.Instance.IsDashing() && Settings.UseQ && SpellManager.Q.IsReady() && !Player.Instance.HasQ3())
            {
                var minionQ =
                    EntityManager.MinionsAndMonsters.EnemyMinions.OrderByDescending(m => m.Health).FirstOrDefault(
                        m =>
                            m.IsEnemy && m.IsInRange(m.GetAfterEPos(), Q.Range) && m.Health <= SpellDamage.QDamage(m));
                if (minionQ != null)
                {
                    SpellManager.Q.Cast(minionQ);
                }

            }
        }
    }
}
