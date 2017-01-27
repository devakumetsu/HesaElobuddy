using EloBuddy;
using EloBuddy.SDK;

using Settings = KickassSeries.Champions.Ahri.Config.Modes.Harass;

namespace KickassSeries.Champions.Ahri.Modes
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
            if (target == null || target.IsZombie) return;

            if (E.IsReady() && target.IsValidTarget(E.Range) && Settings.UseE && Settings.ManaHarass < Player.Instance.ManaPercent)
            {
                E.Cast(target);
            }

            if (W.IsReady() && target.IsValidTarget(W.Range) && Settings.UseW && Settings.ManaHarass < Player.Instance.ManaPercent)
            {
                W.Cast();
            }

            if (Q.IsReady() && target.IsValidTarget(Q.Range) && Settings.UseQ && Settings.ManaHarass < Player.Instance.ManaPercent)
            {
                Q.Cast(target);
            }
        }
    }
}
