using EloBuddy;
using EloBuddy.SDK;

using Settings = KickassSeries.Champions.Blitzcrank.Config.Modes.Combo;

namespace KickassSeries.Champions.Blitzcrank.Modes
{
    public sealed class Combo : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo);
        }

        public override void Execute()
        {
            var target = TargetSelector.GetTarget(Q.Range, DamageType.Magical);
            if (target == null || target.IsZombie) return;

            if (Q.IsReady() && target.IsValidTarget(Q.Range) && Settings.UseQ && !E.IsReady())
            {
                Q.Cast(target);
            }

            if (E.IsReady() && target.IsValidTarget(E.Range) && Settings.UseE)
            {
                E.Cast();
            }

            if (W.IsReady() && target.IsValidTarget(W.Range) && Settings.UseW)
            {
                W.Cast();
            }

            if (R.IsReady() && target.IsValidTarget(R.Range) && Settings.UseR && Player.Instance.CountEnemiesInRange(R.Range) >= 2)
            {
                R.Cast();
            }
        }
    }
}
