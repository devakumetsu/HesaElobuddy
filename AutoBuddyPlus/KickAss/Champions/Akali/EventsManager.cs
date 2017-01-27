using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;

namespace KickassSeries.Champions.Akali
{
    internal static class EventsManager
    {
        public static void Initialize()
        {
            Gapcloser.OnGapcloser += Gapcloser_OnGapcloser;
        }

        private static void Gapcloser_OnGapcloser(AIHeroClient sender, Gapcloser.GapcloserEventArgs e)
        {
            if (!sender.IsEnemy) return;

            if (sender.IsValidTarget(SpellManager.R.Range) && SpellManager.W.IsReady())
            {
                SpellManager.W.Cast(Player.Instance);
            }
        }
    }
}
