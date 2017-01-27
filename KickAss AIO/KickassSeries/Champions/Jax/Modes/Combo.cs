using EloBuddy;
using EloBuddy.SDK;

using Settings = KickassSeries.Champions.Jax.Config.Modes.Combo;

namespace KickassSeries.Champions.Jax.Modes
{
    public sealed class Combo : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo);
        }

        public override void Execute()
        {
            var target = TargetSelector.GetTarget(Q.Range, DamageType.Mixed);
            if (target == null || target.IsZombie) return;

            if (Q.IsReady() && target.IsValidTarget(Q.Range) && Settings.UseQ && !target.IsInRange(Player.Instance, Player.Instance.GetAutoAttackRange()))
            {
                Q.Cast(target);
            }

            if (EventsManager.CanW && target.IsValidTarget(W.Range) && Settings.UseW)
            {
                W.Cast();
            }

            if (E.IsReady() && target.IsValidTarget(E.Range) && Settings.UseE)
            {
                E.Cast();
            }

            if (R.IsReady() && Player.Instance.CountEnemiesInRange(R.Range) >= 2 && Settings.UseR && Player.Instance.HealthPercent <= 60)
            {
                R.Cast();
            }
        }
    }
}
