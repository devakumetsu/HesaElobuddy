using System;
using EloBuddy;
using EloBuddy.SDK;

using Settings = KickassSeries.Champions.Irelia.Config.Modes.Combo;

namespace KickassSeries.Champions.Irelia.Modes
{
    public sealed class Combo : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo);
        }

        private static int _lastRCast;

        public override void Execute()
        {
            var target = TargetSelector.GetTarget(Q.Range, DamageType.Physical);
            if (target == null || target.IsZombie || target.HasUndyingBuff()) return;

            if (Q.IsReady() && Settings.UseQ && target.IsValidTarget(Q.Range) && !target.IsInRange(Player.Instance, Player.Instance.GetAutoAttackRange()))
            {
                Q.Cast(target);
            }

            if (EventsManager.CanW && Settings.UseW)
            {
                W.Cast();
            }

            if (E.IsReady() && Settings.UseE && target.IsValidTarget(E.Range) && (target.HealthPercent > Player.Instance.HealthPercent || target.IsInRange(Player.Instance, Player.Instance.GetAutoAttackRange())))
            {
                E.Cast(target);
            }

            if (R.IsReady() && Settings.UseR && target.IsValidTarget(R.Range) && target.HealthPercent <= 50 && _lastRCast < Environment.TickCount + 50 /*Settings.DelayR*/)
            {
                R.Cast(target);
                _lastRCast = Environment.TickCount;
            }
        }
    }
}
