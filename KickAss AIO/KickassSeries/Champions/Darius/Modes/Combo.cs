using EloBuddy;
using EloBuddy.SDK;

using Settings = KickassSeries.Champions.Darius.Config.Modes.Combo;

namespace KickassSeries.Champions.Darius.Modes
{
    public sealed class Combo : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo);
        }

        public override void Execute()
        {
            var target = TargetSelector.GetTarget(E.Range, DamageType.Physical);
            if (target == null || target.IsZombie) return;

            if (E.IsReady() && target.IsValidTarget(E.Range) && Settings.UseE)
            {
                E.Cast(target);
            }

            if (Q.IsReady() && target.IsValidTarget(Q.Range) && Settings.UseQ)
            {
                Q.Cast();
            }

            if (W.IsReady() && target.IsValidTarget(W.Range) && Settings.UseW)
            {
                W.Cast();
            }

            if (R.IsReady() && target.IsValidTarget(R.Range) && Settings.UseR && target.Health < SpellDamage.GetRealDamage(SpellSlot.R, target))
            {
                R.Cast(target);
            }
        }
    }
}
