using System;
using EloBuddy;
using EloBuddy.SDK.Rendering;

using Settings = KickassSeries.Champions.Jayce.Config.Modes.Draw;

namespace KickassSeries.Champions.Jayce
{
     class Jayce
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
            if (Settings.DrawQ && Settings.DrawReady ? SpellManager.Qg.IsReady() : Settings.DrawQ)
            {
                Circle.Draw(Settings.QColor, SpellManager.Qg.Range, 1f, Player.Instance);
            }

            if (Settings.DrawW && Settings.DrawReady ? SpellManager.Wg.IsReady() : Settings.DrawW)
            {
                Circle.Draw(Settings.WColor, SpellManager.Wg.Range, 1f, Player.Instance);
            }

            if (Settings.DrawE && Settings.DrawReady ? SpellManager.Eg.IsReady() : Settings.DrawE)
            {
                Circle.Draw(Settings.EColor, SpellManager.Eg.Range, 1f, Player.Instance);
            }

            if (Settings.DrawR && Settings.DrawReady ? SpellManager.R.IsReady() : Settings.DrawR)
            {
                Circle.Draw(Settings.RColor, SpellManager.R.Range, 1f, Player.Instance);
            }
        }
    }
}
