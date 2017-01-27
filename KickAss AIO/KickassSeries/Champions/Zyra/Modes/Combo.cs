using EloBuddy;
using EloBuddy.SDK;

using Settings = KickassSeries.Champions.Zyra.Config.Modes.Combo;

namespace KickassSeries.Champions.Zyra.Modes
{
    public sealed class Combo : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo);
        }

        public override void Execute()
        {
            var target = TargetSelector.GetTarget(Q.Range, DamageType.Magical);
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

            if (R.IsReady() && target.IsValidTarget(R.Range) && Settings.UseR)
            {
                R.Cast(target);
            }
        }
    }
}
