using EloBuddy;
using EloBuddy.SDK;

using Settings = KickassSeries.Champions.KogMaw.Config.Modes.Harass;

namespace KickassSeries.Champions.KogMaw.Modes
{
    public sealed class Harass : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass);
        }

        public override void Execute()
        {
            var target = TargetSelector.GetTarget(Q.Range, DamageType.Magical);
            if (target == null || target.IsZombie) return;

            if (E.IsReady() && target.IsValidTarget(E.Range) && Settings.UseE)
            {
                E.Cast(target);
            }

            if (Q.IsReady() && target.IsValidTarget(Q.Range) && Settings.UseQ)
            {
                Q.Cast(target);
            }

            if (EventsManager.CanW && target.IsValidTarget(W.Range) && Settings.UseW)
            {
                W.Cast();
            }
        }
    }
}
