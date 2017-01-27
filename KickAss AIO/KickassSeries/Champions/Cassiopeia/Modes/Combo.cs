using EloBuddy;
using EloBuddy.SDK;

using Settings = KickassSeries.Champions.Cassiopeia.Config.Modes.Combo;

namespace KickassSeries.Champions.Cassiopeia.Modes
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

            if (Q.IsReady() && target.IsValidTarget(Q.Range) && Settings.UseQ)
            {
                Q.Cast(target);
            }

            if (E.IsReady() && target.IsValidTarget(E.Range) && Settings.UseE && target.HasBuffOfType(BuffType.Poison))
            {
                E.Cast(target);
            }

            if (W.IsReady() && target.IsValidTarget(W.Range) && Settings.UseW && !target.HasBuffOfType(BuffType.Poison))
            {
                W.Cast(target);
            }

            if (R.IsReady() && Settings.UseR && target.IsFacing(Player.Instance) && target.IsValidTarget(R.Range) && target.HealthPercent < 20)
            {
                R.Cast(target);
            }
        }
    }
}
