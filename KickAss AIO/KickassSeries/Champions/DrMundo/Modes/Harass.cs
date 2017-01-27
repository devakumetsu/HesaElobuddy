using EloBuddy;
using EloBuddy.SDK;

using Settings = KickassSeries.Champions.DrMundo.Config.Modes.Harass;

namespace KickassSeries.Champions.DrMundo.Modes
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

            if (Settings.UseQ && Q.IsReady() && target.IsValidTarget(Q.Range))
            {
                {
                    Q.Cast(target);
                }
            }
            if (Settings.UseW && W.IsReady() && target.IsValidTarget(W.Range) && !Player.Instance.HasBuff("burningagony"))
            {
                {
                    W.Cast();
                }
            }
            if (Settings.UseE && E.IsReady() && target.IsValidTarget(E.Range))
            {
                {
                    E.Cast();
                }
            }
        }
    }
}
