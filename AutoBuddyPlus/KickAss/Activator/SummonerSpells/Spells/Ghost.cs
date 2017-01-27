using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using KickassSeries.Activator.NoMenuDMGHandler;

using Settings = KickassSeries.Activator.Maps.SummonersRift.Config.Types.SummonerSpells;

namespace KickassSeries.Activator.SummonerSpells.Spells
{
    internal class Ghost
    {
        public static Spell.Active SummonerGhost;
        public static void Initialize()
        {
            var ghost = Player.Instance.Spellbook.Spells.FirstOrDefault(spell => spell.Name.Contains("ghost"));
            if (ghost != null)
            {
                SummonerGhost = new Spell.Active(ghost.Slot);
            }
        }

        public static void Execute()
        {
            if (!SummonerGhost.IsReady() || Activator.lastUsed + 1000 >= Environment.TickCount || !Settings.UseGhost) return;

            if (Player.Instance.InDanger(20))
            {
                SummonerGhost.Cast();
                Activator.lastUsed = Environment.TickCount + 500;
            }
        }
    }
}
