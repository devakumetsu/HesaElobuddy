using EloBuddy;
using EloBuddy.SDK;

namespace KickassSeries.Champions.Irelia
{
    internal static class EventsManager
    {
        public static bool CanW;

        public static void Initialize()
        {
            Orbwalker.OnPreAttack += Orbwalker_OnPreAttack;
        }

        private static void Orbwalker_OnPreAttack(AttackableUnit target, Orbwalker.PreAttackArgs args)
        {
            CanW = SpellManager.W.IsReady();
        }
    }
}
