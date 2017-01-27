using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;

namespace KickassSeries.Champions.Jax
{
    internal static class EventsManager
    {
        public static bool CanW;
        public static void Initialize()
        {
            GameObject.OnCreate += GameObject_OnCreate;
            Gapcloser.OnGapcloser += Gapcloser_OnGapcloser;
            Interrupter.OnInterruptableSpell += Interrupter_OnInterruptableSpell;
            Orbwalker.OnPostAttack += Orbwalker_OnPostAttack;
        }

        private static void GameObject_OnCreate(GameObject sender, System.EventArgs args)
        {
            var minion = sender as Obj_AI_Minion;
            if (minion != null && minion.Name.Contains("Ward") && SpellManager.Q.IsReady())
            {
                if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Flee))
                {
                    Core.DelayAction(() => SpellManager.Q.Cast(minion), 50);
                }
            }
        }

        private static void Orbwalker_OnPostAttack(AttackableUnit target, System.EventArgs args)
        {
            CanW = SpellManager.W.IsReady();
            Core.DelayAction(() => CanW = false, 80);
        }

        private static void Gapcloser_OnGapcloser(AIHeroClient sender, Gapcloser.GapcloserEventArgs e)
        {
            if (!sender.IsEnemy) return;

            if (sender.IsValidTarget(SpellManager.E.Range))
            {
                SpellManager.E.Cast();
            }
        }

        private static void Interrupter_OnInterruptableSpell(Obj_AI_Base sender, Interrupter.InterruptableSpellEventArgs e)
        {
            if (!sender.IsEnemy) return;

            if (e.DangerLevel == DangerLevel.High && sender.IsValidTarget(SpellManager.E.Range))
            {
                SpellManager.E.Cast();
            }
        }
    }
}
