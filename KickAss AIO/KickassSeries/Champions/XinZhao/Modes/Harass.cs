using EloBuddy;
using EloBuddy.SDK;

using Settings = KickassSeries.Champions.XinZhao.Config.Modes.Harass;

namespace KickassSeries.Champions.XinZhao.Modes
{
    public sealed class Harass : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass);
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
        }
    }
}
