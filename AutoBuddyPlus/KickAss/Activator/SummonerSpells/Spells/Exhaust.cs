using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using KickassSeries.Activator.NoMenuDMGHandler;

namespace KickassSeries.Activator.SummonerSpells.Spells
{
    internal class Exhaust
    {
        public static Spell.Targeted SummonerExhaust;
        public static void Initialize()
        {
            var ghost = Player.Instance.Spellbook.Spells.FirstOrDefault(spell => spell.Name.Contains("exhaust"));
            if (ghost != null)
            {
                SummonerExhaust = new Spell.Targeted(ghost.Slot, 650);
            }
        }

        public static void Execute()
        {
            if (!SummonerExhaust.IsReady() || Activator.lastUsed >= Environment.TickCount) return;
            var target = TargetSelector.GetTarget(SummonerExhaust.Range, DamageType.Mixed);

            var Ally = EntityManager.Heroes.Allies.FirstOrDefault(a => a.IsInRange(Player.Instance, SummonerExhaust.Range));
            if (Player.Instance.InDanger(30) || Ally.InDanger(30))
            {
                SummonerExhaust.Cast(target);
                Activator.lastUsed = Environment.TickCount + 500;
            }
        }
    }
}
