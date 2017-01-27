using EloBuddy;
using EloBuddy.SDK;

using Settings = KickassSeries.Champions.Viktor.Config.Modes.Harass;

namespace KickassSeries.Champions.Viktor.Modes
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
                E.Cast(target);
            }

            if (W.IsReady() && target.IsValidTarget(W.Range) && Settings.UseW)
            {
                W.Cast(target);
            }

            if (Q.IsReady() && target.IsValidTarget(Q.Range) && Settings.UseQ)
            {
                Q.Cast();
            }
        }
    }
}
