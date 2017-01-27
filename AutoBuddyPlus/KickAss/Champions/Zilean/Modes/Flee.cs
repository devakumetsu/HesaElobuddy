using EloBuddy;
using EloBuddy.SDK;

namespace KickassSeries.Champions.Zilean.Modes
{
    public sealed class Flee : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Flee);
        }

        public override void Execute()
        {
            var target = TargetSelector.GetTarget(Q.Range, DamageType.Magical);
            if (target == null || target.IsZombie) return;

            if (Q.IsReady() && target.IsValidTarget(Q.Range) && !E.IsReady())
            {
                Q.Cast(target);
            }

            if (E.IsReady())
            {
                E.Cast(Player.Instance);
            }

            if (W.IsReady() && target.IsValidTarget(W.Range) && !Q.IsReady())
            {
                W.Cast();
            }
        }
    }
}
