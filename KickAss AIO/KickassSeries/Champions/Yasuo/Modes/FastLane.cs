using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using Settings = KickassSeries.Champions.Yasuo.Config.Modes.Misc;

namespace KickassSeries.Champions.Yasuo.Modes
{
    public sealed class FastLane : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return true; /*Settings.FastLaneKey;*/
        }

        public override void Execute()
        {
            Orbwalker.MoveTo(Game.CursorPos);

            var minion =
                EntityManager.MinionsAndMonsters.EnemyMinions.OrderByDescending(m => m.Health).FirstOrDefault(
                    m => m.IsValidTarget(Player.Instance.AttackRange));

            Player.IssueOrder(GameObjectOrder.AttackUnit, minion);

            if (SpellManager.E.IsReady())
            {
                var minionE =
                    EntityManager.MinionsAndMonsters.EnemyMinions.OrderByDescending(m => m.Health)
                        .FirstOrDefault(m => m.IsEnemy && m.IsValidTarget(SpellManager.E.Range));

                if (minionE != null && !minionE.GetAfterEPos().Tower())
                {
                    SpellManager.E.Cast(minionE);
                }
            }

            if (SpellManager.Q.IsReady() && !Player.Instance.HasQ3())
            {
                var minionQ =
                    EntityManager.MinionsAndMonsters.EnemyMinions.OrderByDescending(m => m.Health)
                        .FirstOrDefault(m => m.IsEnemy && m.IsValidTarget(SpellManager.Q.Range));
                if (minionQ != null)
                {
                    SpellManager.Q.Cast(minionQ);
                }
            }
        }
    }
}
