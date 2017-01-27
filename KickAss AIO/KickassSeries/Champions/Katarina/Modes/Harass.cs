using EloBuddy;
using EloBuddy.SDK;

using Settings = KickassSeries.Champions.Katarina.Config.Modes.Harass;

namespace KickassSeries.Champions.Katarina.Modes
{
    public sealed class Harass : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass);
        }

        public override void Execute()
        {
            var target = TargetSelector.GetTarget(SpellManager.Q.Range, DamageType.Magical);
            if (target == null || target.CountEnemiesInRange(SpellManager.R.Range) <= 2) return;

            if (SpellManager.Q.IsReady() && Settings.UseQ)
            {
                SpellManager.Q.Cast(target);
            }

            if (SpellManager.E.IsReady() && target.IsValidTarget(SpellManager.E.Range) && Settings.UseE &&
                target.HealthPercent + 15 > Player.Instance.HealthPercent)
            {
                SpellManager.E.Cast(target);
            }

            if (SpellManager.W.IsReady() && target.IsValidTarget(SpellManager.W.Range) && Settings.UseW)
            {
                SpellManager.W.Cast();
            }
        }
    }
}
