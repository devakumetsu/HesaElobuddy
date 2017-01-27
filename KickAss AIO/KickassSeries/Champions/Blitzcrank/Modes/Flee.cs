using EloBuddy;
using EloBuddy.SDK;

namespace KickassSeries.Champions.Blitzcrank.Modes
{
    public sealed class Flee : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Flee);
        }

        public override void Execute()
        {
            if(Player.Instance.CountEnemiesInRange(Q.Range) < 1 && Player.Instance.HealthPercent >20)return;
             
            if (W.IsReady())
            {
                W.Cast();
            }
        }
    }
}
