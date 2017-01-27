using EloBuddy;
using EloBuddy.SDK;

namespace KickassSeries.Champions.ChoGath.Modes
{
    public sealed class PermaActive : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return true;
        }

        public override void Execute()
        {
            var target = TargetSelector.GetTarget(R.Range, DamageType.Magical);
            if (target == null || target.IsZombie) return;

            if (R.IsReady() && target.IsValidTarget(R.Range) && target.Health <= SpellDamage.GetRealDamage(SpellSlot.R, target))
            {
                R.Cast(target);
            }
        }
    }
}
