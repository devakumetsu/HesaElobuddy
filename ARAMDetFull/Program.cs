using System;
using System.Drawing;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using static ARAMDetFull.ARAMDetFull;

namespace ARAMDetFull
{
    class Program
    {
        public static bool DebugCrash = true;

        public static void Main(string[] args)
        {
            Game.OnTick += OnTick;
            new ARAMDetFull();
            
        }

        private static void OnTick(EventArgs args)
        {
            var pig = Player.Instance.GetAutoAttackRange();
            var target = ARAMTargetSelector.getBestTarget(Player.Instance.GetAutoAttackRange(),true);
            //var target = TargetSelector.GetTarget(Player.Instance.GetAutoAttackRange(),DamageType.Physical,Player.Instance.Position);
            var bTarg = EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy, Player.Instance.Position, Player.Instance.GetAutoAttackRange()).OrderBy(x => x.Distance(Player.Instance)).FirstOrDefault();
            if (Player.Instance.IsInRange(target,pig) && Player.Instance.CountEnemiesInRange(550) <= 1 && Player.Instance.CountAlliesInRange(550) <=1)
            {
                Player.IssueOrder(GameObjectOrder.AttackUnit, target);
            }

            else
            {
                if (Player.Instance.CountEnemiesInRange(500) <= 0 && Player.Instance.CountEnemyMinionsInRange(500) <=1)
                {
                    Player.IssueOrder(GameObjectOrder.AttackUnit, bTarg);
                }
            }
        }
    }
}