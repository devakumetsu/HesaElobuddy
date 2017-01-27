using EloBuddy;
using EloBuddy.SDK;

using Settings = KickassSeries.Champions.Bard.Config.Modes.Harass;

namespace KickassSeries.Champions.Bard.Modes
{
    public sealed class Harass : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass);
        }

        public override void Execute()
        {
            var target = TargetSelector.GetTarget(Q.Range, DamageType.Magical);
            if (target == null || target.IsZombie) return;

            if (Q.IsReady() && target.IsValidTarget(Q.Range) && Settings.UseQ)
            {
                Functions.CastQ(target);
            }
        }
    }
}
