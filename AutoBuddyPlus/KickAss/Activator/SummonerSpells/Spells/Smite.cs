using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;

using KickassSeries.Activator.Maps.Summoner;
using KickassSeries.Activator.Maps.SummonersRift;
using Settings = KickassSeries.Activator.Maps.SummonersRift.Config.Types.SummonerSpells;

namespace KickassSeries.Activator.SummonerSpells.Spells
{
    // ReSharper disable once ClassNeverInstantiated.Global
    internal class Smite
    {
        public static Spell.Targeted SummonerSmite;

        private static readonly string[] SmiteableUnits =
        {
            "SRU_Red", "SRU_Blue", "SRU_Dragon", "SRU_Baron",
            "SRU_Gromp", "SRU_Murkwolf", "SRU_Razorbeak",
            "SRU_Krug", "Sru_Crab"
        };


        public static void Initialize()
        {
            var smite = Player.Instance.Spellbook.Spells.FirstOrDefault(spell => spell.Name.Contains("smite"));
            if (smite != null)
            {
                SummonerSmite = new Spell.Targeted(smite.Slot, 500);
            }
        }

        private static int GetSmiteDamage()
        {
            var level = Player.Instance.Level;

            int[] smitedamage =
            {
                20*level + 370,
                30*level + 330,
                40*level + 240,
                50*level + 100
            };
            return smitedamage.Max();
        }

        public static void Execute()
        {
            if (!SummonerSmite.IsReady() || !Settings.Smite || Player.Instance.IsDead || Player.Instance.IsRecalling() ||
                SummonerSpells.Initialize.lastSpell + 1500 >= Environment.TickCount) return;

            var jugMonster =
                EntityManager.MinionsAndMonsters.Monsters.OrderBy(m => m.MaxHealth)
                    .FirstOrDefault(
                        a =>
                            SmiteableUnits.Contains(a.BaseSkinName) && a.Health <= GetSmiteDamage() &&
                            Config.Types.SummonerMenu[a.BaseSkinName].Cast<CheckBox>().CurrentValue);

            if (jugMonster != null)
            {
                SummonerSmite.Cast(jugMonster);
                SummonerSpells.Initialize.lastSpell = Environment.TickCount;
            }

            if (Settings.KsSmite &&
                SummonerSmite.Handle.Name == "s5_summonersmiteplayerganker")
            {
                var target =
                    EntityManager.Heroes.Enemies
                        .FirstOrDefault(
                            h => h.IsValidTarget(SummonerSmite.Range) && h.Health <= 20 + 8*Player.Instance.Level);
                if (target != null)
                {
                    SummonerSmite.Cast(target);
                    SummonerSpells.Initialize.lastSpell = Environment.TickCount;
                }
            }

            if (!Settings.DuelSmite || SummonerSmite.Handle.Name != "s5_summonersmiteduel" ||
                !Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo)) return;

            var targetDuel =
                EntityManager.Heroes.Enemies
                    .OrderByDescending(TargetSelector.GetPriority)
                    .FirstOrDefault(h => h.IsValidTarget(SummonerSmite.Range));

            if (targetDuel == null) return;

            SummonerSmite.Cast(targetDuel);
            SummonerSpells.Initialize.lastSpell = Environment.TickCount;
        }
    }
}
