using System.Linq;
using EloBuddy;
using EloBuddy.SDK;

namespace KickassSeries.Champions.Thresh.Modes
{
    public sealed class Flee : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Flee);
        }

        public override void Execute()
        {
            if (W.IsReady())
            {
                var ally =
                    EntityManager.Heroes.Allies.OrderByDescending(h => h.Health)
                        .FirstOrDefault(a => a.IsValidTarget(W.Range));

                if (ally != null)
                {
                    W.Cast(ally);
                }
            }

            var target = TargetSelector.GetTarget(E.Range, DamageType.Mixed);
            if (target == null) return;
            if (E.IsReady())
            {
                E.Cast(target);
            }
        }
    }
}