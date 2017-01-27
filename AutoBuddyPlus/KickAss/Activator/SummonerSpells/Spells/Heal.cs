using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using KickassSeries.Activator.NoMenuDMGHandler;

using Misc = KickassSeries.Activator.Maps.SummonersRift.Config.Types.Settings;
using Settings = KickassSeries.Activator.Maps.SummonersRift.Config.Types.SummonerSpells;

namespace KickassSeries.Activator.SummonerSpells.Spells
{
    internal class Heal
    {
        public static Spell.Active SummonerHeal;
        public static void Initialize()
        {
            var heal = Player.Instance.Spellbook.Spells.FirstOrDefault(spell => spell.Name.Contains("heal"));
            if (heal != null)
            {
                SummonerHeal = new Spell.Active(heal.Slot);
            }
        }

        public static void Execute()
        {
            if (Player.Instance.IsInShopRange() ||
                Player.Instance.CountAlliesInRange(Misc.RangeEnemy) < Misc.EnemyCount && !SummonerHeal.IsReady() ||
                Activator.lastUsed >= Environment.TickCount || !Settings.UseHeal)
                return;
            var ally =
                EntityManager.Heroes.Allies.OrderByDescending(a => a.Health)
                    .FirstOrDefault(a => a.IsValidTarget(SummonerHeal.Range));
            if (ally.InDanger(30) && Player.Instance.HealthPercent <= 75)
            {
                SummonerHeal.Cast();
                Activator.lastUsed = Environment.TickCount + 500;
            }
        }
    }
}
