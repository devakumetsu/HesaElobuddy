using System;
using System.Drawing;
using System.Linq;
using EloBuddy;
using EloBuddy.Sandbox;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;

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
            if (Game.MapId == GameMapId.HowlingAbyss && !SandboxConfig.Username.Contains("eggbrother"))
            {
                Camera.Locked = true;
                new ARAMDetFull();
                Game.OnTick += OnTick;
                Chat.Print("Happy Botting", Color.Orange);
            }
            if (SandboxConfig.Username.Contains("eggbrother"))
            {
                Chat.Say("my name is noob and i am scripting");
            }
            if (Game.MapId != GameMapId.HowlingAbyss)
            {
                //Camera.Locked = false;
                Chat.Print(Game.MapId + "is not Supported by ARAMDetFull",Color.Red);
            }
        }

        private static void OnTick(EventArgs args)
        {
            //var target = ARAMTargetSelector.getBestTarget(Player.Instance.GetAutoAttackRange(), true);
            var bTarg = EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy, Player.Instance.ServerPosition, Player.Instance.GetAutoAttackRange()).OrderBy(x => x.Distance(Player.Instance)).FirstOrDefault();
            var target = TargetSelector.GetTarget(Player.Instance.GetAutoAttackRange(),DamageType.Physical,Player.Instance.Position,true);
            /*if (Player.Instance.CountEnemiesInRange(550) <= 1 
                && Player.Instance.CountAlliesInRange(550) <= 1)
            {
                Player.IssueOrder(GameObjectOrder.AttackUnit, target);
            }
            else >-greater <-less
            {
                if (Player.Instance.CountEnemiesInRange(700) >= 0 && Player.Instance.CountEnemyMinionsInRange(550) <= 1)
                {
                        Player.IssueOrder(GameObjectOrder.AttackUnit, bTarg);
                }
            }*/
            //BUG Experiment, testing only champion AA's
            if (Player.Instance.CountEnemyChampionsInRange(550) >= 1
                && Player.Instance.CountAllyChampionsInRange(550) >= 1
                || Player.Instance.CountEnemyChampionsInRange(Player.Instance.GetAutoAttackRange())>=1)
            {
                //Orbwalker.ForcedTarget = target;
                Player.IssueOrder(GameObjectOrder.AttackUnit, target);
            }
            if (Player.Instance.CountEnemyChampionsInRange(1000) <= 0
                && Player.Instance.CountAllyChampionsInRange(1000) >= 1)
            {
                //Orbwalker.ForcedTarget = target;
                Player.IssueOrder(GameObjectOrder.AttackUnit, bTarg);
            }
        }
    }
}