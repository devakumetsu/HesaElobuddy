using System.Linq;
using EloBuddy;
using EloBuddy.SDK;

namespace KickassSeries.Champions.Akali.Modes
{
    public sealed class Flee : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Flee);
        }

        public override void Execute()
        {
            if (R.IsReady() && Player.Instance.HealthPercent <= 35 && Player.Instance.CountEnemiesInRange(R.Range) >= 1)
            {
                var enemyminion =
                    EntityManager.MinionsAndMonsters.EnemyMinions.OrderByDescending(m => m.Distance(Game.CursorPos))
                        .FirstOrDefault(m => m.IsValidTarget(R.Range));
                if (enemyminion == null)return;

                R.Cast(enemyminion);
            }
        }
    }
}
