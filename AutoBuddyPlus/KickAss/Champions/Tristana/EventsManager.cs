using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;

using Misc = KickassSeries.Champions.Tristana.Config.Modes.Misc;

namespace KickassSeries.Champions.Tristana
{
    internal static class EventsManager
    {
        public static void Initialize()
        {
            Gapcloser.OnGapcloser += Gapcloser_OnGapcloser;
            Interrupter.OnInterruptableSpell += Interrupter_OnInterruptableSpell;
        }

        private static void Gapcloser_OnGapcloser(AIHeroClient sender, Gapcloser.GapcloserEventArgs e)
        {
            if (!sender.IsEnemy) return;

            if (sender.IsEnemy && sender.IsInRange(Player.Instance, SpellManager.R.Range) && Player.Instance.Distance(e.End) < SpellManager.R.Range && Misc.AntiGapCloser)
            {
                SpellManager.R.Cast(sender);
            }
        }

        private static void Interrupter_OnInterruptableSpell(Obj_AI_Base sender, Interrupter.InterruptableSpellEventArgs e)
        {
            if (!sender.IsEnemy) return;

            if (sender.IsValidTarget(SpellManager.R.Range) && e.DangerLevel >= DangerLevel.High && Misc.InterruptSpell)
            {
                SpellManager.R.Cast(sender);
            }
        }
    }
}
