using EloBuddy;
using EloBuddy.SDK;
using KickassSeries.Ultilities;

using Settings = KickassSeries.Champions.Tristana.Config.Modes.Combo;

namespace KickassSeries.Champions.Tristana.Modes
{
    public sealed class PermaActive : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return true;
        }

        public override void Execute()
        {
            var target = TargetSelector.GetTarget(R.Range, DamageType.Physical);
            if (target == null || target.IsZombie) return;

            if (R.IsReady() && Settings.UseR)
            {
                var stacks = target.GetBuffCount("tristanaecharge");
                if (stacks > 0)
                {
                    if (target.Health <= (SpellDamage.GetRealDamage(SpellSlot.E, target) * ((0.3 * stacks) + 1) +
                                          SpellDamage.GetRealDamage(SpellSlot.R, target)))
                    {
                        R.Cast(target);
                    }
                }
            }

            if (R.IsReady() && Settings.UseR)
            {
                if (target.Health <= (SpellDamage.GetRealDamage(SpellSlot.R, target)) && target.Health > Player.Instance.TotalAttackDamage)
                {
                    R.Cast(target);
                }
            }

            if (R.IsReady()/* && Settings.UseRTower*/)
            {
                if (target.RPos().AllyTower())
                {
                    R.Cast(target);
                }
            }
        }
    }
}
