using System.Linq;
using EloBuddy;
using EloBuddy.SDK;

namespace KickassSeries.Champions.Bard.Modes
{
    public sealed class PermaActive : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return true;
        }

        public override void Execute()
        {
            var target = TargetSelector.GetTarget(Q.Range, DamageType.Magical);
            if (target == null || target.IsZombie) return;

            if (W.IsReady())
            {
                var ally = EntityManager.Heroes.Allies.OrderByDescending(a => a.Health).FirstOrDefault(a => a.IsValidTarget(W.Range) && a.HealthPercent <= 10);
                if (ally != null)
                {
                    W.Cast(ally);
                }
            }
        }
    }
}
