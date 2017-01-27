using System;
using System.Collections.Generic;
using System.Linq;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;

//using Settings = KickassSeries.Ultilities.Config.Types.SkinHack;


namespace KickassSeries.Ultilities
{
    internal static class SkinHack
    {
        /*
        public static readonly Dictionary<string, int> Skins = new Dictionary<string, int>();

        private static int lastCheck;

        public static void Execute()
        {
            if (lastCheck + 5000 > Environment.TickCount || Settings.TurnOff) return;
            CheckSkin();
            lastCheck = Environment.TickCount;
        }

        private static void CheckSkin()
        {
            if (Settings.TurnOff) return;
            var enemies =
                EntityManager.Heroes.Enemies.Where(
                    e => e.SkinId != Config.Types.SkinHackMenu["kaenemy" + e.ChampionName].Cast<Slider>().CurrentValue);

            if (enemies != null)
            {
                foreach (var enemy in enemies)
                {
                    Skins[enemy.Name] = Config.Types.SkinHackMenu["kaenemy" + enemy.ChampionName].Cast<Slider>().CurrentValue;
                    enemy.SetSkin(enemy.ChampionName, Skins[enemy.Name]);
                }
            }

            var allies =
                EntityManager.Heroes.Allies.Where(
                    a => a.SkinId != Config.Types.SkinHackMenu["kaally" + a.ChampionName].Cast<Slider>().CurrentValue);
            if (allies != null)
            {
                foreach (var ally in allies)
                {
                    Skins[ally.Name] = Config.Types.SkinHackMenu["kaally" + ally.ChampionName].Cast<Slider>().CurrentValue;
                    ally.SetSkin(ally.ChampionName, Skins[ally.Name]);
                }
            }
        }
        */
    }
}
