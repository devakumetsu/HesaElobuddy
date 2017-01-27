using EloBuddy;
using EloBuddy.SDK;

using Settings = KickassSeries.Champions.Xerath.Config.Modes.Combo;

namespace KickassSeries.Champions.Xerath.Modes
{
    public sealed class Combo : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo);
        }

        public override void Execute()
        {
            var target = TargetSelector.GetTarget(Q.MaximumRange, DamageType.Magical);
            if (target == null || target.IsZombie) return;

            if (target.IsValidTarget(Q.MaximumRange) && Q.IsReady() && !Q.IsCharging && Settings.UseQ)
            {
                Q.StartCharging();
            }

            if (Q.IsCharging)
            {
                if (target.IsValidTarget(Q.Range + 30))
                {
                    Q.Cast(target);
                }
            }

            if(Q.IsCharging) return;

            if (E.IsReady() && target.IsValidTarget(E.Range) && Settings.UseE)
            {
                E.Cast(target);
            }

            if (W.IsReady() && target.IsValidTarget(W.Range) && Settings.UseW)
            {
                W.Cast(target);
            }

            if (R.IsReady() && target.IsValidTarget(R.Range) && Settings.UseR && target.Health < SpellDamage.GetRealDamage(SpellSlot.R, target))
            {
                R.Cast();
            }

            if (PermaActive.IsCastingUlt && R.Handle.Ammo > 0)
            {
                R.Cast(target);
            }
        }
    }
}
