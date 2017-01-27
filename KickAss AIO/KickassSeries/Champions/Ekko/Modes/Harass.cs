using EloBuddy;
using EloBuddy.SDK;

using Settings = KickassSeries.Champions.Ekko.Config.Modes.Harass;

namespace KickassSeries.Champions.Ekko.Modes
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

            if (W.IsReady() && Settings.UseW)
            {
                W.Cast(Player.Instance.Position.Extend(target.Position, W.Range).To3D());
            }

            if (E.IsReady() && target.IsValidTarget(E.Range) && Settings.UseE)
            {
                E.Cast(target);
            }

            if (Q.IsReady() && target.IsValidTarget(Q.Range) && Settings.UseQ)
            {
                Q.Cast(target);
            }
        }
    }
}
