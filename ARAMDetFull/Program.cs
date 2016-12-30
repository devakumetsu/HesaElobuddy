using System;
using System.Drawing;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;

namespace ARAMDetFull
{
    class Program
    {
        public static bool DebugCrash = true;

        public static void Main(string[] args)
        {
            new ARAMDetFull();
            Game.OnTick += OnTick;
            Drawing.OnDraw += OnDraw;
        }

        private static void OnTick(EventArgs args)
        {
            var target = ARAMTargetSelector.getBestTarget(550, true);
            //var target = TargetSelector.GetTarget(Player.Instance.GetAutoAttackRange(), DamageType.Mixed,Player.Instance.Position);
            var bTarg = EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy, Player.Instance.ServerPosition, Player.Instance.GetAutoAttackRange()).OrderBy(x => x.Distance(Player.Instance)).FirstOrDefault();
            if (Player.Instance.CountEnemiesInRange(550) <= 1 && Player.Instance.CountAlliesInRange(550) <=1)
            {
                Player.IssueOrder(GameObjectOrder.AttackUnit, target);
            }
            else
            {
                if (Player.Instance.CountEnemiesInRange(1000) == 0 && Player.Instance.CountAlliesInRange(1000) <= 1)
                {
                    Player.IssueOrder(GameObjectOrder.AttackUnit, bTarg);
                }
            }
        }

        private static void OnDraw(EventArgs args)
        {
            Drawing.DrawCircle(Player.Instance.Position, 1000, Color.Red);
            Drawing.DrawCircle(Player.Instance.Position, 550, Color.White);
        }
    }
}