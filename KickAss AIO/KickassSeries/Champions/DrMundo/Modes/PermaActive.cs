using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using KickassSeries.Activator;
using Settings = KickassSeries.Champions.DrMundo.Config.Modes.Misc;

namespace KickassSeries.Champions.DrMundo.Modes
{
    public sealed class PermaActive : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return true;
        }

        public override void Execute()
        {
            if (Player.Instance.HasBuff("burningagony"))
            {
                var target = TargetSelector.GetTarget(W.Range, DamageType.Physical);

                var minions =
                    EntityManager.MinionsAndMonsters.Minions.OrderBy(m => m.Distance(Player.Instance))
                        .FirstOrDefault(m => m.IsValidTarget(300) && m.IsEnemy);

                if (target == null && minions == null)
                {
                    W.Cast();
                }
            }
            /*
            if (Settings.AutoR && Player.Instance.HealthPercent <= Settings.HealthR)
            {
                    R.Cast();
                var target = TargetSelector.GetTarget(600, DamageType.Physical);
                if (target == null) return;

                R.Cast();
            }
            */
        }
    }
}
