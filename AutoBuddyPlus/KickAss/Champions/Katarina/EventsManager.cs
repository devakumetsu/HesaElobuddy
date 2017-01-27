using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;
using KickassSeries.Champions.Katarina.Modes;

namespace KickassSeries.Champions.Katarina
{
    internal static class EventsManager
    {
        public static void Initialize()
        {
            Gapcloser.OnGapcloser += Gapcloser_OnGapcloser;
            GameObject.OnCreate += Obj_AI_Base_OnCreate;
        }

        private static void Obj_AI_Base_OnCreate(GameObject sender, System.EventArgs args)
        {
            var minion = sender as Obj_AI_Minion;
            if (minion != null && minion.Name.Contains("Ward") && SpellManager.E.IsReady())
            {
                if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Flee))
                {
                    Core.DelayAction(() => SpellManager.E.Cast(minion), 50);
                }
            }
        }

        private static void Gapcloser_OnGapcloser(AIHeroClient sender, Gapcloser.GapcloserEventArgs e)
        {
            if (!sender.IsEnemy) return;

            if (sender.IsValidTarget(SpellManager.E.Range))
            {
                Flee.WardJump(Player.Instance.Position.Shorten(sender.Position, SpellManager.E.Range));
            }
        }
    }
}
