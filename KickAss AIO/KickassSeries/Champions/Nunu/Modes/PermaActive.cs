using System.Linq;
using EloBuddy;
using EloBuddy.SDK;

namespace KickassSeries.Champions.Nunu.Modes
{
    public sealed class PermaActive : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return true;
        }

        public override void Execute()
        {
            //Enable Orbwalker
            if (Player.Instance.Spellbook.IsChanneling)
            {
                Orbwalker.DisableAttacking = true;
                Orbwalker.DisableMovement = true;
            }
            else
            {
                Orbwalker.DisableAttacking = false;
                Orbwalker.DisableMovement = false;
            }
            //Enable Orbwalker

            //SAVE ME PLEASE Q
            var minionQ =
                EntityManager.MinionsAndMonsters.EnemyMinions.OrderByDescending(m => m.Distance(Player.Instance)).FirstOrDefault(m => m.IsValidTarget(1000));

            if (minionQ == null)return;

            if (Player.Instance.HealthPercent <= 20 && Q.IsReady())
            {
                if (minionQ.IsValidTarget(Q.Range))
                {
                    Q.Cast(minionQ);
                }
                else
                {
                    Player.IssueOrder(GameObjectOrder.MoveTo, minionQ);
                }
            }
            //SAVE ME PLEASE Q

        }
    }
}
