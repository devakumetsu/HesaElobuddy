using EloBuddy;
using EloBuddy.SDK;

namespace KickassSeries.Champions.Kayle.Modes
{
    public sealed class Flee : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Flee);
        }

        public override void Execute()
        {
            var target = TargetSelector.GetTarget(W.Range, DamageType.Magical);
            if (Player.Instance.HealthPercent <= 35 && target.IsValidTarget(W.Range))
            {
                W.Cast(Player.Instance);
            }
        }
    }
}
