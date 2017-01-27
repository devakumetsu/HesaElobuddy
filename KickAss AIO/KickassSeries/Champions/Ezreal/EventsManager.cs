using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;

using Misc = KickassSeries.Champions.Ezreal.Config.Modes.Misc;

using Settings = KickassSeries.Champions.Ezreal.Config.Modes.LastHit;

namespace KickassSeries.Champions.Ezreal
{
    internal static class EventsManager
    {
        public static bool CanQCancel;
        public static bool CanWCancel;

        public static void Initialize()
        {
            Gapcloser.OnGapcloser += Gapcloser_OnGapcloser;
            Orbwalker.OnPostAttack += Orbwalker_OnPostAttack;
            Orbwalker.OnUnkillableMinion += Orbwalker_OnUnkillableMinion;
        }

        private static void Orbwalker_OnUnkillableMinion(Obj_AI_Base target, Orbwalker.UnkillableMinionArgs args)
        {/*
            var minion = target as Obj_AI_Minion;
            if (minion != null && minion.IsValidTarget(SpellManager.Q.Range) && Settings.UseQun && Player.Instance.ManaPercent >= Settings.ManaLast)
            {
                SpellManager.Q.Cast(minion);
            }*/
        }

        private static void Orbwalker_OnPostAttack(AttackableUnit target, System.EventArgs args)
        {
            CanQCancel = SpellManager.Q.IsReady();
            if (!CanWCancel)
            {
                CanWCancel = SpellManager.W.IsReady();
            }
        }

        private static void Gapcloser_OnGapcloser(AIHeroClient sender, Gapcloser.GapcloserEventArgs e)
        {/*
            if (!Misc.GapE && !SpellManager.E.IsReady() && sender.IsAlly) return;

            if (sender.IsEnemy && sender.IsVisible && Player.Instance.Distance(e.End) < 150)
            {
                SpellManager.E.Cast(Player.Instance.Position.Shorten(sender.Position, SpellManager.E.Range));
            }*/
        }
    }
}
