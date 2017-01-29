using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.IO;
using System.Reflection;
using AutoBuddy.Humanizers;
using AutoBuddy.MainLogics;
using AutoBuddy.MyChampLogic;
using AutoBuddy.Utilities;
using AutoBuddy.Utilities.AutoLvl;
using AutoBuddy.Utilities.AutoShop;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using SharpDX;
using Version = System.Version;

namespace AutoBuddy
{
    internal static class Program
    {
        private static Menu menu;
        private static IChampLogic myChamp;
        private static LogicSelector Logic { get; set; }
        static List<string> _allowed = new List<string> { "/noff", "/ff", "/mute all", "/msg", "/r", "/w", "/surrender", "/nosurrender", "/help", "/dance", "/d", "/taunt", "/t", "/joke", "/j", "/laugh", "/l" };

        private static void Main(string[] args)
        {
            // fix for aram map; and conflict with AramBuddy 
            if (Game.MapId != GameMapId.SummonersRift)
            {
                Chat.Print(Game.MapId + " IS NOT Supported By AutoBuddyPlus");
                return;
            }

            Loading.OnLoadingComplete += Loading_OnLoadingComplete;
        }

        private static void Loading_OnLoadingComplete(EventArgs args)
        {
            createFS();
            var randomTime = new Random().Next(8000, 15000);
            Chat.Print("<font size='30'>AB+</font> <font color='#b756c5'>customized by Hesa</font>");
            Chat.Print("Starting in " + ((int) randomTime / 1000) +" seconds.");
            Chat.Print("If you like botting consider to try www.HesaBot.com");

            Core.DelayAction(Start, randomTime);
            menu = MainMenu.AddMenu("AB+", "AB");
            menu.AddGroupLabel("Default");

            PropertyInfo property2 = typeof(CheckBox).GetProperty("Size");

            Slider sliderLanes = menu.Add("lane", new Slider(" ", 1, 1, 4));
            string[] lanes =
            {
                "", "Selected lane: Auto", "Selected lane: Top", "Selected lane: Mid",
                "Selected lane: Bot"
            };
            sliderLanes.DisplayName = lanes[sliderLanes.CurrentValue];
            sliderLanes.OnValueChange += delegate(ValueBase<int> sender, ValueBase<int>.ValueChangeArgs changeArgs)
            {
                sender.DisplayName = lanes[changeArgs.NewValue];
            };

            menu.Add("disablepings", new CheckBox("Disable pings", false));
            menu.Add("disablechat", new CheckBox("Disable chat", false));

            menu.AddSeparator(5);
            menu.AddGroupLabel("Surrender");
            Slider sliderFF = menu.Add("ff", new Slider(" ", 2, 1, 2));
            string[] ffStrings =
            {
                "", "Surrender Vote: Yes", "Surrender Vote: No"
            };
            sliderFF.DisplayName = ffStrings[sliderFF.CurrentValue];
            sliderFF.OnValueChange += delegate(ValueBase<int> sender, ValueBase<int>.ValueChangeArgs changeArgs)
            {
                sender.DisplayName = ffStrings[changeArgs.NewValue];
            };

            menu.AddSeparator(5);
            menu.AddGroupLabel("Others");
            CheckBox newpf = new CheckBox("Use smart pathfinder", true);
            menu.Add("newPF", newpf);
            newpf.OnValueChange += newpf_OnValueChange;
            menu.Add("reselectlane", new CheckBox("Reselect lane", false));

            menu.AddLabel("----------------------------");
            menu.Add("autoclose", new CheckBox("Auto close lol. Need Reload (F5)", true));
            menu.Add("autoshop", new CheckBox("Enable AutoShop", true));
            menu.Add("oldWalk", new CheckBox("Use old orbwalk. Need Reload (F5)", true));
            menu.Add("debuginfo", new CheckBox("Draw debug info", false));
            menu.Add("l1", new Label("By Christian Brutal Sniper - Updated by Hesa, Tryller and DevAkumetsu"));
            
        }
        
        static void newpf_OnValueChange(ValueBase<bool> sender, ValueBase<bool>.ValueChangeArgs args)
        {
            AutoWalker.newPF = args.NewValue;
        }

        private static void Start()
        {
            RandGen.Start();
            /*
            bool generic = false;
            switch (ObjectManager.Player.Hero)
            {
                case Champion.Aatrox:
                    myChamp = new Aatrox();
                break;
                case Champion.Ashe:
                    myChamp = new Ashe();
                break;
                case Champion.Caitlyn:
                    myChamp = new Caitlyn();
                break;
                case Champion.Cassiopeia:
                    myChamp = new Cassiopeia();
                break;
                case Champion.Ezreal:
                    myChamp = new Ezreal();
                break;
                case Champion.Jinx:
                    myChamp = new Jinx();
                break;
                case Champion.Sivir:
                    myChamp = new Sivir();
                break;
                case Champion.Tristana:
                    myChamp = new Tristana();
                break;
                case Champion.Vayne:
                    myChamp = new Vayne();
                break;

                default:
                    generic = true;
                    myChamp = new Generic();
                break;
            }*/
            var generic = true;
            myChamp = new Generic();
            CustomLvlSeq cl = new CustomLvlSeq(menu, AutoWalker.myHero, Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "EloBuddy\\AutoBuddyPlus\\Skills"));
            
            Logic = new LogicSelector(myChamp, menu);

            Game.OnTick += OnTick;
            KickAss.KickAss.Initialize();
        }

        public static void OnTick(EventArgs args)
        {
            AutoShop.OnTick(args);
            AutoWalker.OnTick(args);
        }

        private static void createFS()
        {
            Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "EloBuddy\\AutoBuddyPlus"));
            Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "EloBuddy\\AutoBuddyPlus\\Builds"));
            Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "EloBuddy\\AutoBuddyPlus\\Skills"));
        }
    }
}