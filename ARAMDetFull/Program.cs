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
            var target = ARAMTargetSelector.getBestTarget(Player.Instance.GetAutoAttackRange(), true);
            //var target = TargetSelector.GetTarget(Player.Instance.GetAutoAttackRange(),DamageType.Physical,Player.Instance.Position);
            if (Player.Instance.IsInRange(target, Player.Instance.GetAutoAttackRange()) && Player.Instance.CountEnemiesInRange(550) <= 1 &&
                Player.Instance.CountAlliesInRange(550) <= 1)
            {
                Player.IssueOrder(GameObjectOrder.AttackUnit, target);
            }
            /*else
            {
                if (Player.Instance.CountEnemiesInRange(550) >= 0)
                {
                    Player.IssueOrder(GameObjectOrder.AttackUnit, bTarg);
                }
            }*/
        }
    }
}