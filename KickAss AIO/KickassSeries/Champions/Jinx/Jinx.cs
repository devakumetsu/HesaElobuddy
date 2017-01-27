using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;

using Settings = KickassSeries.Champions.Jinx.Config.Modes.Draw;
using Misc = KickassSeries.Champions.Jinx.Config.Modes.Misc;

namespace KickassSeries.Champions.Jinx
{
    internal class Jinx
    {
        public static void Initialize()
        {
            Config.Initialize();
            SpellManager.Initialize();
            ModeManager.Initialize();
            DamageIndicator.Initialize(SpellDamage.GetTotalDamage);
            EventsManager.Initialize();

            Drawing.OnDraw += OnDraw;
        }

         private static void OnDraw(EventArgs args)
         {
            if (Settings.DrawQ && Settings.DrawReady ? SpellManager.Q.IsReady() : Settings.DrawQ)
            {
                Circle.Draw(Settings.QColor, SpellManager.Q.Range, 1f, Player.Instance);
            }

            if (Settings.DrawW && Settings.DrawReady ? SpellManager.W.IsReady() : Settings.DrawW)
            {
                Circle.Draw(Settings.WColor, SpellManager.W.Range, 1f, Player.Instance);
            }

            if (Settings.DrawE && Settings.DrawReady ? SpellManager.E.IsReady() : Settings.DrawE)
            {
                Circle.Draw(Settings.EColor, SpellManager.E.Range, 1f, Player.Instance);
            }

            if (Settings.DrawR && Settings.DrawReady ? SpellManager.R.IsReady() : Settings.DrawR)
            {
                Circle.Draw(Settings.RColor, SpellManager.R.Range, 1f, Player.Instance);
            }

            if (true/*Settings.DrawWPred*/)
             {
                 var enemy =
                     EntityManager.Heroes.Enemies.Where(t => t.IsValidTarget() && SpellManager.W.IsInRange(t))
                         .OrderBy(t => t.Distance(Player.Instance))
                         .FirstOrDefault();
                 if (enemy == null)
                 {
                     return;
                 }
                 var wPred = SpellManager.W.GetPrediction(enemy).CastPosition;
                 Essentials.DrawLineRectangle(wPred.To2D(), Player.Instance.Position.To2D(), SpellManager.W.Width, 1,
                     SpellManager.W.IsReady() ? System.Drawing.Color.YellowGreen : System.Drawing.Color.Red);
             }

             if (true /*Settings.DrawRPred*/)
             {
                 var enemy =
                     EntityManager.Heroes.Enemies.Where(
                         t =>
                             t.IsValidTarget()
                             && t.Distance(Player.Instance) >= 500 /*Misc.RRange*/ &&
                             t.Distance(Player.Instance) <= SpellManager.R.Range)
                         .OrderBy(t => t.Distance(Player.Instance))
                         .FirstOrDefault();
                 if (enemy == null)
                 {
                     return;
                 }
                 var rPred = SpellManager.R.GetPrediction(enemy).CastPosition;
                 Essentials.DrawLineRectangle(rPred.To2D(), Player.Instance.Position.To2D(), SpellManager.R.Width, 1,
                     SpellManager.R.IsReady() ? System.Drawing.Color.YellowGreen : System.Drawing.Color.Red);
             }
         }
    }
}
