using System;
using System.Drawing;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using static ARAMDetFull.ARAMDetFull;

namespace ARAMDetFull
{
    class Program
    {
        public static bool DebugCrash = true;

        public static void Main(string[] args)
        {
            //new ARAMDetFull();
            //Game.OnLoad += OnLoad;
           // Game.OnTick += OnTick;
            Loading.OnLoadingComplete += OnLoad;

        }

        private static void OnLoad(EventArgs args)
        {
            new ARAMDetFull();
            Game.OnTick += OnTick;
        }

        private static void OnTick(EventArgs args)
        {
            var target = ARAMTargetSelector.getBestTarget(Player.Instance.GetAutoAttackRange(), true);
            var bTarg = EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy, Player.Instance.ServerPosition, Player.Instance.GetAutoAttackRange()).OrderBy(x => x.Distance(Player.Instance)).FirstOrDefault();
            //var target = TargetSelector.GetTarget(Player.Instance.GetAutoAttackRange(), DamageType.Physical, Player.Instance.Position);
            if (Player.Instance.CountEnemiesInRange(550) <= 1 
                && Player.Instance.CountAlliesInRange(550) <= 1)
            {
                Player.IssueOrder(GameObjectOrder.AttackUnit, target);
            }
            else
            {
                if (Player.Instance.CountEnemiesInRange(550) >= 0 && Player.Instance.CountEnemyMinionsInRange(550) <= 1)
                {
                        Player.IssueOrder(GameObjectOrder.AttackUnit, bTarg);
                }
            }
        }
    }
}