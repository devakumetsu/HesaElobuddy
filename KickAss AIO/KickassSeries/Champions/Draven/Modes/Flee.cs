using EloBuddy;
using EloBuddy.SDK;

namespace KickassSeries.Champions.Draven.Modes
{
    public sealed class Flee : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Flee);
        }

        public override void Execute()
        {
            var target = TargetSelector.GetTarget(Q.Range, DamageType.Magical);
            if (Player.Instance.HealthPercent <= 25 && target.IsValidTarget(E.Range))
            {
                E.Cast(target);
            }
        }
    }
}
