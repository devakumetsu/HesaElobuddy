using EloBuddy;
using EloBuddy.SDK;

using Settings = KickassSeries.Champions.Alistar.Config.Modes.Combo;

namespace KickassSeries.Champions.Alistar.Modes
{
    public sealed class Combo : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo);
        }

        public override void Execute()
        {
            var target = TargetSelector.GetTarget(W.Range, DamageType.Magical);
            if (target == null || target.IsZombie) return;

            if (W.IsReady() && target.IsValidTarget(W.Range) && Settings.UseW)
            {
                W.Cast(target);
            }

            if (Q.IsReady() && target.IsValidTarget(Q.Range) && Settings.UseQ)
            {
                Q.Cast();
            }

            if (E.IsReady() && target.IsValidTarget(E.Range) && Settings.UseE)
            {
                E.Cast();
            }

            if (R.IsReady() && target.IsValidTarget(800) && Settings.UseR && Player.Instance.HealthPercent <= 30)
            {
                R.Cast();
            }
        }
    }
}
