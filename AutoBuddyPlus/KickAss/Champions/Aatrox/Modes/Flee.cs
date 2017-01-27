using EloBuddy;
using EloBuddy.SDK;

namespace KickassSeries.Champions.Aatrox.Modes
{
    public sealed class Flee : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Flee);
        }

        public override void Execute()
        {
            var target = TargetSelector.GetTarget(Q.Range, DamageType.Physical);
            if (target == null || target.IsZombie) return;
            if (Q.IsReady())
            {
                Q.Cast(Player.Instance.Position.Extend(Game.CursorPos, Q.Range).To3D());
            }
            if (E.IsReady() && target.IsValidTarget(E.Range) && Player.Instance.HealthPercent >= 10 || !target.IsInRange(Player.Instance, 250))
            {
                E.Cast(target);
            }
        }
    }
}
