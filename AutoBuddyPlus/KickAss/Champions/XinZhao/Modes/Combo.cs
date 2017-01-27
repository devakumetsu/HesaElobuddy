using EloBuddy;
using EloBuddy.SDK;

using Settings = KickassSeries.Champions.XinZhao.Config.Modes.Combo;

namespace KickassSeries.Champions.XinZhao.Modes
{
    public sealed class Combo : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo);
        }

        public override void Execute()
        {
            var target = TargetSelector.GetTarget(E.Range, DamageType.Physical);
            if (target == null || target.IsZombie) return;

            if (E.IsReady() && target.IsValidTarget(E.Range) && Settings.UseE)
            {
                E.Cast(target);
            }

            if (EventsManager.CanW && target.IsValidTarget(W.Range) && Settings.UseW)
            {
                W.Cast();
            }

            if (EventsManager.CanQ && target.IsValidTarget(Q.Range) && Settings.UseQ)
            {
                Q.Cast();
            }

            if (R.IsReady() && Settings.UseR && Player.Instance.CountEnemiesInRange(R.Range) >= 2 || target.HealthPercent <= 20)
            {
                R.Cast();
            }
        }
    }
}
