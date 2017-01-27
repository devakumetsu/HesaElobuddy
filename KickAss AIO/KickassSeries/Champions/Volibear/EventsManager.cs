using EloBuddy;
using EloBuddy.SDK;

namespace KickassSeries.Champions.Volibear
{
    internal static class EventsManager
    {
        public static bool CanR;

        public static void Initialize()
        {
            Orbwalker.OnPreAttack += Orbwalker_OnPreAttack;
        }

        private static void Orbwalker_OnPreAttack(AttackableUnit target, Orbwalker.PreAttackArgs args)
        {
            CanR = SpellManager.R.IsReady();
        }
    }
}
