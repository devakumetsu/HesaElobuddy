using EloBuddy;
using EloBuddy.SDK;

using Settings = KickassSeries.Champions.Aatrox.Config.Modes.Combo;

namespace KickassSeries.Champions.Aatrox.Modes
{
    public sealed class Combo : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo);
        }

        public override void Execute()
        {
            var target = TargetSelector.GetTarget(Q.Range, DamageType.Physical);
            if (target == null || target.IsZombie) return;

            if (Q.IsReady() && target.IsValidTarget(Q.Range) && Settings.UseQ)
            {
                Q.Cast(target);
            }

            if (E.IsReady() && target.IsValidTarget(E.Range) && Settings.UseE)
            {
                E.Cast(target);
            }

            if (R.IsReady() && target.IsValidTarget(R.Range) && Settings.UseR &&
                target.HealthPercent + 10 > Player.Instance.HealthPercent && target.HealthPercent <= 30)
            {
                R.Cast();
            }
        }
    }
}
