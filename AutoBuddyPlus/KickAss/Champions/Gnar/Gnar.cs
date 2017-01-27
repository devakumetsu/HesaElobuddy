using System;
using EloBuddy;
using EloBuddy.SDK.Rendering;

using Settings = KickassSeries.Champions.Gnar.Config.Modes.Draw;

namespace KickassSeries.Champions.Gnar
{
     class Gnar
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
            if (Settings.DrawQ && Settings.DrawReady ? SpellManager.Q1.IsReady() : Settings.DrawQ)
            {
                Circle.Draw(Settings.QColor, SpellManager.Q1.Range, 1f, Player.Instance);
            }

            if (Settings.DrawW && Settings.DrawReady ? SpellManager.W2.IsReady() : Settings.DrawW)
            {
                Circle.Draw(Settings.WColor, SpellManager.W2.Range, 1f, Player.Instance);
            }

            if (Settings.DrawE && Settings.DrawReady ? SpellManager.E1.IsReady() : Settings.DrawE)
            {
                Circle.Draw(Settings.EColor, SpellManager.E1.Range, 1f, Player.Instance);
            }

            if (Settings.DrawR && Settings.DrawReady ? SpellManager.R.IsReady() : Settings.DrawR)
            {
                Circle.Draw(Settings.RColor, SpellManager.R.Range, 1f, Player.Instance);
            }
        }
    }
}
