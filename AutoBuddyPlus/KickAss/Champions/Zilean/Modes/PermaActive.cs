using System.Linq;
using EloBuddy.SDK;
using KickassSeries.Activator;

namespace KickassSeries.Champions.Zilean.Modes
{
    public sealed class PermaActive : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return true;
        }

        public override void Execute()
        {
            if (!R.IsReady()) return;
            var ally = EntityManager.Heroes.Allies.FirstOrDefault(a => a.HealthPercent <= 15 && a.IsMe);
            if (ally == null) return;
            R.Cast(ally);
        }
    }
}
