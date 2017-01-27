using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using SharpDX;
using Settings = KickassSeries.Champions.Ziggs.Config.Modes.LaneClear;

namespace KickassSeries.Champions.Ziggs.Modes
{
    public sealed class JungleClear : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear);
        }

        public override void Execute()
        {
            var target = TargetSelector.GetTarget(Q3.Range, DamageType.Magical);
            if (target == null || target.IsZombie) return;

            if (Q1.IsReady() && target.IsValidTarget(Q3.Range) && Settings.UseQ && !E.IsReady())
            {
                Functions.CastQ(target);
            }

            if (E.IsReady() && target.IsValidTarget(E.Range) && Settings.UseE)
            {
                E.Cast(target);
            }

            if (W.IsReady() && target.IsValidTarget(W.Range) && Settings.UseW)
            {
                W.Cast();
            }
        }
    }
}
