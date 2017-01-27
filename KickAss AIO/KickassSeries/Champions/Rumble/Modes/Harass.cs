using EloBuddy;
using EloBuddy.SDK;

using Settings = KickassSeries.Champions.Rumble.Config.Modes.Harass;

namespace KickassSeries.Champions.Rumble.Modes
{
    public sealed class Harass : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass);
        }

        public override void Execute()
        {
            var target = TargetSelector.GetTarget(E.Range, DamageType.Physical);
            if (target == null || target.IsZombie) return;

            if (E.IsReady() && target.IsValidTarget(E.Range) && Settings.UseE)
            {
                E.Cast(E.GetPrediction(target).CastPosition);
            }

            if (Q.IsReady() && target.IsValidTarget(Q.Range) && Settings.UseQ && Player.Instance.IsFacing(target))
            {
                Q.Cast();
            }
        }
    }
}
