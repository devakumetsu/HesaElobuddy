using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using EloBuddy.SDK.Events;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Menu;
using System.Collections.Generic;

namespace ARAMDetFull
{
    class ARAMDetFull
    {
        /* TODO:
         * ##- Tower range higher dives a lot
         * ##- before level 6/7 play safer dont go so close stay behind other players or 800/900 units away from closest enemy champ
         * ##- Target selector based on invincible enemies
         * ##- IF invincible or revive go full in
         * ##- if attacking enemy and it is left 3 or less aa to kill then follow to kill (check movespeed dif)
         *  - bush invis manager player death
         *  - fixx gankplank plays like retard
         *  - this weeks customs
         *  - WPF put to allways take mark
         * ##- nami auto level
         *  - Some skills make aggresivity for time and how much to put in balance ignore minsions on/off
         * ## - LeeSin
         * ## - Nocturn
         *  - Gnar
         *  -Katarina error
         *  - Gangplank error
         *  ##- healing relics
         *  -Make velkoz
         */
        public static TextWriter defaultOut;
        public delegate void OnGameEndHandler(bool win);

        public ARAMDetFull()
        {
            Console.WriteLine("Aram det full started!");
            Loading.OnLoadingComplete += onLoad;
        }
        
        public static int gameStart = 0;

        public static Menu Config, Extra, Debug;

        public static int now
        {
            get { return (int)DateTime.Now.TimeOfDay.TotalMilliseconds; }
        }

        public static List<int> allyIds;
        public static List<int> enemyIds;

        private static void onLoad(object sender)
        {
            r = new Random();
            allyIds = new List<int>();
            enemyIds = new List<int>();

            gameStart = now;
            Chat.Print("<font size='30'>AramDetFull</font> <font color='#b756c5'>ported by Hesa & Ouija</font>");
            try
            {
                defaultOut = Console.Out;
                Console.SetOut(new ErrorLogger(defaultOut));
                
                Config = MainMenu.AddMenu("ARAM", "aramDet");

                Extra = Config.AddSubMenu("Extra", "aramDetExtra");
                Extra.Add("debugDraw", new CheckBox("Debug draw", false));
                Extra.Add("dataGathering", new CheckBox("Send errors to server", false));

                Debug = Config.AddSubMenu("Debug", "aramDetDebug");
                Debug.Add("botOff", new CheckBox("Bot off", false));
                Debug.Add("db_targ", new KeyBind("Debug Target", false, KeyBind.BindTypes.PressToggle, 'T'));
                
                Drawing.OnDraw += onDraw;
                Game.OnUpdate += OnGameUpdate;
                Game.OnNotify += Game_OnNotify;
                
                var gameEndNotified = false;

                OnGameEnd += ARAMDetFull_OnGameEnd;

                Game.OnTick += delegate
                {
                    if (gameEndNotified)
                    {
                        return;
                    }
                    var nexus = ObjectManager.Get<Obj_HQ>();
                    if (nexus == null)
                    {
                        return;
                    }
                    if (nexus.Any(n => n.IsDead || n.Health.Equals(0)))
                    {
                        var win = ObjectManager.Get<Obj_HQ>().Any(n => n.Health.Equals(0));
                        OnGameEnd?.Invoke(win);
                        gameEndNotified = true;
                    }
                };

                ARAMSimulator.setupARMASimulator();

                foreach (AIHeroClient ally in EloBuddy.SDK.EntityManager.Heroes.Allies)
                {
                    allyIds.Add(ally.NetworkId);
                }

                foreach (AIHeroClient enemy in EloBuddy.SDK.EntityManager.Heroes.Enemies)
                {
                    enemyIds.Add(enemy.NetworkId);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private static void Game_OnNotify(GameNotifyEventArgs args)
        {
            if (args.EventId == GameEventId.OnChampionKill)
            {
                if (allyIds.Contains((int)args.NetworkId))
                {
                    teamKills++;
                }
                else
                {
                    enemyKills++;
                }
            }
            if (args.EventId == GameEventId.OnChampionPentaKill && allyIds.Contains((int)args.NetworkId))
            {
                Chat.Say("ez");
            }

            if (args.EventId == GameEventId.OnSurrenderVote)
                Core.DelayAction(() => Chat.Say("/ff"), r.Next(2000, 5000));
        }

        public static Random r { get; private set; }

        public static int teamKills;
        public static int enemyKills;
        
        private static void ARAMDetFull_OnGameEnd(bool win)
        {
            var rnd = new Random().Next(15000, 30000) + Game.Ping;
            Core.DelayAction(() => { Game.QuitGame(); }, rnd);
        }

        private static void onDraw(EventArgs args)
        {
            return;
            try
            {
                if (!Debug["debugDraw"].Cast<CheckBox>().CurrentValue) return;
                Drawing.DrawText(100, 100, Color.Red, "2bal: " + ARAMSimulator.balance+ " fear: "+MapControl.fearDistance );
                
                foreach (var hel in ObjectManager.Get<Obj_AI_Minion>().Where(r => r.IsValid && !r.IsDead && r.Name.ToLower().Contains("blobdrop")))
                {
                    var spos = Drawing.WorldToScreen(hel.Position);
                    Drawing.DrawText(spos.X, spos.Y, Color.Brown, " : " + hel.Name);
                    Drawing.DrawText(spos.X, spos.Y+25, Color.Brown, hel.IsDead + " : " + hel.Type+ " : " + hel.IsValid+ " : " + hel.CharacterState);
                }

                var tar = ARAMTargetSelector.getBestTarget(5100);
                if (tar != null) Drawing.DrawCircle(tar.Position, 150, Color.Violet);

                foreach (var sec in ARAMSimulator.sectors)
                {
                    sec.draw();
                }

                foreach (var ene in MapControl.enemy_champions)
                {
                    var spos = Drawing.WorldToScreen(ene.hero.Position);
                    Drawing.DrawCircle(ene.hero.Position, ene.reach , Color.Green);
                
                    Drawing.DrawText(spos.X, spos.Y, Color.Green,"Gold: "+ene.hero.Gold);
                }
                return;//????

                foreach (var ene in MapControl.enemy_champions)
                {
                    Drawing.DrawCircle(ene.hero.Position, ene.reach, Color.Violet);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }
        public static void getAllBuffs()
        {
            foreach (var aly in EntityManager.Enemies)
            {
                foreach (var buffs in aly.Buffs)
                {
                    Console.WriteLine(aly.BaseSkinName + " - Buf: " + buffs.Name);
                }
            }
        }

        public static event OnGameEndHandler OnGameEnd;
        private static int lastTick = now;

        private static int tickTimeRng = 77;
        private static Random rng = null;
        private static void OnGameUpdate(EventArgs args)
        {
            try
            {
                if (Debug["botOff"].Cast<CheckBox>().CurrentValue)
                    return;
                
                if (Debug["db_targ"].Cast<KeyBind>().CurrentValue)
                {
                    foreach (var buf in ObjectManager.Player.Buffs)
                    {
                        Console.WriteLine(buf.Name);
                    }
                }
                lastTick = now;
                ARAMSimulator.updateArmaPlay();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error at line: " + ex.LineNumber());
                Console.WriteLine(ex);
            }
        }
    }
}