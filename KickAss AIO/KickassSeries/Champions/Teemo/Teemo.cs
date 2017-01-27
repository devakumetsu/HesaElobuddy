using System;
using System.Drawing;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Rendering;
using KickassSeries.Ultilities;

using Settings = KickassSeries.Champions.Teemo.Config.Modes.Draw;
//using Debug = KickassSeries.Champions.Teemo.Config.Modes.Debug;

namespace KickassSeries.Champions.Teemo
{
    internal class Teemo
    {
        public static FileHandler Handler;
        public static ShroomTables shroomPositions;

        public static readonly string[] Marksman =
        {
            "Ashe", "Caitlyn", "Corki", "Draven", "Ezreal", "Jinx", "Kalista",
            "KogMaw", "Lucian", "MissFortune", "Quinn", "Sivir", "Teemo", "Tristana", "Twitch", "Urgot", "Varus",
            "Vayne"
        };

        public static void Initialize()
        {
            Config.Initialize();
            SpellManager.Initialize();
            ModeManager.Initialize();
            DamageIndicator.Initialize(SpellDamage.GetTotalDamage);
            EventsManager.Initialize();

            Drawing.OnDraw += OnDraw;

            Handler = new FileHandler();
            shroomPositions = new ShroomTables();
        }

        private static void OnDraw(EventArgs args)
        {/*
            if (Debug.DebugDraw)
            {
                Drawing.DrawText(
                    Debug.X,
                    Debug.Y,
                    Color.Red,
                    Player.Instance.Position.ToString());
            }

            if (Settings.DrawQ && Settings.DrawReady ? SpellManager.Q.IsReady() : Settings.DrawQ)
            {
                new Circle {Color = Settings.colorQ, BorderWidth = Settings._widthQ, Radius = SpellManager.Q.Range}.Draw
                    (Player.Instance.Position);
            }

            if (Settings.DrawW && Settings.DrawReady ? SpellManager.W.IsReady() : Settings.DrawW)
            {
                new Circle {Color = Settings.colorW, BorderWidth = Settings._widthW, Radius = SpellManager.W.Range}.Draw
                    (Player.Instance.Position);
            }

            if (Settings.DrawE && Settings.DrawReady ? SpellManager.E.IsReady() : Settings.DrawE)
            {
                new Circle {Color = Settings.colorE, BorderWidth = Settings._widthE, Radius = SpellManager.E.Range}.Draw
                    (Player.Instance.Position);
            }

            if (Settings.DrawR && Settings.DrawReady ? SpellManager.R.IsReady() : Settings.DrawR)
            {
                new Circle {Color = Settings.colorR, BorderWidth = Settings._widthR, Radius = SpellManager.R.Range}.Draw
                    (Player.Instance.Position);
            }

            if (Settings.DrawAutoR && Game.MapId == GameMapId.SummonersRift)
            {
                if (!shroomPositions.SummonersRift.Any())
                {
                    return;
                }
                foreach (
                    var place in
                        shroomPositions.SummonersRift.Where(
                            pos => pos.Distance(Player.Instance.Position) <= Settings.DrawVision))
                {
                    {
                        Circle.Draw(place.IsShroomed() ? SharpDX.Color.Red : SharpDX.Color.LightGreen, 100, place);
                    }
                }
            }
            else if (Settings.DrawAutoR && Game.MapId == GameMapId.CrystalScar)
            {
                if (!shroomPositions.CrystalScar.Any())
                {
                    return;
                }
                foreach (
                    var place in
                        shroomPositions.CrystalScar.Where(
                            pos => pos.Distance(Player.Instance.Position) <= Settings.DrawVision))
                {
                    {
                        Circle.Draw(place.IsShroomed() ? SharpDX.Color.Red : SharpDX.Color.LightGreen, 100, place);
                    }
                }
            }
            else if (Settings.DrawAutoR && Game.MapId == GameMapId.HowlingAbyss)
            {
                if (!shroomPositions.HowlingAbyss.Any())
                {
                    return;
                }
                foreach (
                    var place in
                        shroomPositions.HowlingAbyss.Where(
                            pos => pos.Distance(Player.Instance.Position) <= Settings.DrawVision))
                {
                    {
                        Circle.Draw(place.IsShroomed() ? SharpDX.Color.Red : SharpDX.Color.LightGreen, 100, place);
                    }
                }
            }
            else if (Settings.DrawAutoR && Game.MapId == GameMapId.TwistedTreeline)
            {
                if (!shroomPositions.TwistedTreeline.Any())
                {
                    return;
                }
                foreach (
                    var place in
                        shroomPositions.TwistedTreeline.Where(
                            pos => pos.Distance(Player.Instance.Position) <= Settings.DrawVision))
                {
                    {
                        Circle.Draw(place.IsShroomed() ? SharpDX.Color.Red : SharpDX.Color.LightGreen, 100, place);
                    }
                }
            }
            else if (Settings.DrawAutoR && shroomPositions.ButcherBridge.Any())
            {
                foreach (
                    var place in
                        shroomPositions.ButcherBridge.Where(
                            pos =>
                                pos.Distance(Player.Instance.Position)
                                <= Settings.DrawVision))
                {
                    {
                        Circle.Draw(place.IsShroomed() ? SharpDX.Color.Red : SharpDX.Color.LightGreen, 100, place);
                    }
                }
            }*/
        }
    }
}
