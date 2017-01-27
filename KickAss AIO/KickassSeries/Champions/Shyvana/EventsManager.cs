using EloBuddy;
using EloBuddy.SDK;

namespace KickassSeries.Champions.Shyvana
{
    internal static class EventsManager
    {
        public static bool CanQ;
        public static void Initialize()
        {
            Orbwalker.OnPostAttack += Orbwalker_OnPostAttack;
        }

        private static void Orbwalker_OnPostAttack(AttackableUnit target, System.EventArgs args)
        {
            CanQ = SpellManager.Q.IsReady();
            Core.DelayAction(() => CanQ = false, 80);
        }
    }
}
