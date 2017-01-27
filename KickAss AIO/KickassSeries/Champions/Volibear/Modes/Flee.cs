using EloBuddy;
using EloBuddy.SDK;

namespace KickassSeries.Champions.Volibear.Modes
{
    public sealed class Flee : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Flee);
        }

        public override void Execute()
        {
            if (Player.Instance.HealthPercent <= 45 && Player.Instance.CountEnemiesInRange(R.Range) > 1)
            {
                Q.Cast();
            }
        }
    }
}
