using EloBuddy;
using EloBuddy.SDK;

using Settings = KickassSeries.Champions.Irelia.Config.Modes.Harass;

namespace KickassSeries.Champions.Irelia.Modes
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

            if (Q.IsReady() && Settings.UseQ && target.IsValidTarget(Q.Range))
            {
                Q.Cast(target);
            }

            if (EventsManager.CanW && Settings.UseW &&
                (target.HealthPercent > Player.Instance.HealthPercent ||
                 target.IsInRange(Player.Instance, Player.Instance.GetAutoAttackRange())))
            {
                W.Cast();
            }

            if (E.IsReady() && Settings.UseE && target.IsValidTarget(E.Range))
            {
                E.Cast(target);
            }
        }
    }
}
