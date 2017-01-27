using EloBuddy;
using EloBuddy.SDK;

namespace KickassSeries.Champions.Riven.Modes
{
    public sealed class Flee : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Flee);
        }

        public override void Execute()
        {
            if (Player.Instance.HealthPercent <= 15 && Player.Instance.CountEnemiesInRange(R.Range) > 1)
            {
                R.Cast(Player.Instance.Position.Extend(Game.CursorPos, R.Range + 250).To3D());
            }
        }
    }
}
