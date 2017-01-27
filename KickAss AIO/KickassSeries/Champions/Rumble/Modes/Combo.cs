using EloBuddy;
using EloBuddy.SDK;

using Settings = KickassSeries.Champions.Rumble.Config.Modes.Combo;

namespace KickassSeries.Champions.Rumble.Modes
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
            if (target == null || target.IsZombie || target.HasUndyingBuff()) return;

            if (E.IsReady() && target.IsValidTarget(E.Range) && Settings.UseE)
            {
                E.Cast(E.GetPrediction(target).CastPosition);
            }

            if (Q.IsReady() && target.IsValidTarget(Q.Range) && Settings.UseQ && Player.Instance.IsFacing(target))
            {
                Q.Cast();
            }

            /*
            if (R.IsReady() && target.IsValidTarget(R.Range) && Settings.UseR)
            {
                R.Cast(target);
            }
            */
        }
    }
}
