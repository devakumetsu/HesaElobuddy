using EloBuddy;
using EloBuddy.SDK;

using Settings = KickassSeries.Champions.Ezreal.Config.Modes.Harass;

namespace KickassSeries.Champions.Ezreal.Modes
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

            if (Settings.UseQ && Q.IsReady() && target.IsValidTarget(Q.Range) &&
                !target.IsInRange(Player.Instance, Player.Instance.GetAutoAttackRange()) &&
                Settings.ManaHarass <= Player.Instance.ManaPercent)
            {
                Q.Cast(target);
            }

            if (Settings.UseQ && EventsManager.CanQCancel && target.IsValidTarget(Q.Range) &&
                target.IsInRange(Player.Instance, Player.Instance.GetAutoAttackRange()) &&
                Settings.ManaHarass <= Player.Instance.ManaPercent)
            {
                Q.Cast(target);
            }

            if (Settings.UseW && W.IsReady() && target.IsValidTarget(W.Range) &&
                !target.IsInRange(Player.Instance, Player.Instance.GetAutoAttackRange()) &&
                Settings.ManaHarass <= Player.Instance.ManaPercent)
            {
                W.Cast(target);
            }

            if (Settings.UseW && EventsManager.CanWCancel && target.IsValidTarget(W.Range) &&
                target.IsInRange(Player.Instance, Player.Instance.GetAutoAttackRange()) &&
                Settings.ManaHarass <= Player.Instance.ManaPercent)
            {
                W.Cast(target);
            }
        }
    }
}
