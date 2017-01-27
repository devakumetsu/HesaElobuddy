using EloBuddy;
using EloBuddy.SDK;

using Settings = KickassSeries.Champions.Tristana.Config.Modes.Harass;

namespace KickassSeries.Champions.Tristana.Modes
{
    public sealed class Harass : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass);
        }

        public override void Execute()
        {
            var target = TargetSelector.GetTarget(Q.Range, DamageType.Physical);
            if (target == null || target.IsZombie || target.HasUndyingBuff()) return;

            Orbwalker.ForcedTarget = null;

            if (Settings.UseE && E.IsReady() && target.IsValidTarget(E.Range))
            {
                E.Cast(target);
            }

            if (Settings.UseQ && target.IsValidTarget(Q.Range) && target.GetBuffCount("tristanaecharge") > 0)
            {
                Q.Cast();
            }
        }
    }
}
