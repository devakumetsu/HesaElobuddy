using EloBuddy;
using EloBuddy.SDK;

namespace KickassSeries.Champions.XinZhao
{
    internal static class EventsManager
    {
        public static bool CanQ;
        public static bool CanW;

        public static void Initialize()
        {
            Orbwalker.OnPreAttack += Orbwalker_OnPreAttack;
        }

        private static void Orbwalker_OnPreAttack(AttackableUnit target, Orbwalker.PreAttackArgs args)
        {
            CanQ = SpellManager.Q.IsReady();
            CanW = SpellManager.W.IsReady();
        }
    }
}
