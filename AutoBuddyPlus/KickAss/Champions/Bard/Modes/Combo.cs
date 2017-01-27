using System.Linq;
using EloBuddy;
using EloBuddy.SDK;

using Settings = KickassSeries.Champions.Bard.Config.Modes.Combo;

namespace KickassSeries.Champions.Bard.Modes
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
                Functions.CastQ(target);
            }

            if (W.IsReady() && Settings.UseW)
            {
                var ally = EntityManager.Heroes.Allies.OrderByDescending(a => a.Health).FirstOrDefault(a => a.IsValidTarget(W.Range) && a.HealthPercent <= 15);
                if(ally != null)
                {
                    W.Cast(ally);
                }
            }

            if (R.IsReady() && target.IsValidTarget(R.Range) && Settings.UseR && target.CountEnemiesInRange(R.Width) >= 2)
            {
                R.Cast(target);
            }
        }
    }
}
