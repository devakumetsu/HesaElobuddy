using EloBuddy;
using EloBuddy.SDK;

namespace KickassSeries.Champions.Corki.Modes
{
    public sealed class Flee : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Flee);
        }

        public override void Execute()
        {
            if (Player.Instance.HealthPercent <= 35 && Player.Instance.CountEnemiesInRange(R.Range) > 1)
            {
                W.Cast(Player.Instance.Position.Extend(Game.CursorPos, W.Range).To3D());
            }
        }
    }
}
