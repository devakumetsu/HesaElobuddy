using EloBuddy;
using EloBuddy.SDK;

using Settings = KickassSeries.Champions.Corki.Config.Modes.Combo;

namespace KickassSeries.Champions.Corki.Modes
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

            if (Q.IsReady() && target.IsValidTarget(Q.Range) && Settings.UseQ)
            {
                Q.Cast(target);
            }

            if (W.IsReady() && target.IsValidTarget(W.Range) && Settings.UseW)
            {
                W.Cast(target);
            }

            if (E.IsReady() && target.IsValidTarget(E.Range) && Settings.UseE && Player.Instance.IsFacing(target))
            {
                E.Cast();
            }

            if (R.IsReady() && target.IsValidTarget(R.Range) && Settings.UseR)
            {
                R.Cast(target);
            }
        }
    }
}
