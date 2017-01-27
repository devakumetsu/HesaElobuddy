using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;

using Settings = KickassSeries.Champions.Teemo.Config.Modes.LaneClear;

namespace KickassSeries.Champions.Teemo.Modes
{
    public sealed class LaneClear : ModeBase
    {
        private static int LaneClearLastR;

        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear);
        }

        public override void Execute()
        {
            var minion =
                EntityManager.MinionsAndMonsters.GetLaneMinions()
                    .OrderByDescending(m => m.Health)
                    .FirstOrDefault(m => m.IsValidTarget(Q.Range));

            if (minion == null) return;


            var qMinion =
                EntityManager.MinionsAndMonsters.EnemyMinions.Where(
                    t => Q.IsInRange(t) && t.IsValidTarget() && t.IsMinion && t.IsEnemy)
                    .OrderBy(t => t.Health)
                    .FirstOrDefault();

            if (qMinion != null)
            {
                if (Q.IsReady()
                    && Settings.UseQ
                    && qMinion.Health <= Player.Instance.GetSpellDamage(qMinion, SpellSlot.Q)
                    /*&& Settings.QMana <= (int) Player.Instance.ManaPercent*/)
                {
                    Q.Cast(qMinion);
                }
            }

            if (!Settings.UseR)
            {
                return;
            }

            var allMinionsR =
                EntityManager.MinionsAndMonsters.EnemyMinions.Where(t => R.IsInRange(t) && t.IsValidTarget())
                    .OrderBy(t => t.Health);
            var rLocation = EntityManager.MinionsAndMonsters.GetCircularFarmLocation(allMinionsR, R.Width, (int) R.Range);
            /*
            if (rLocation.HitNumber >= Settings.rMinions
                && Environment.TickCount - LaneClearLastR >= 5000)
            {
                R.Cast(rLocation.CastPosition);
                LaneClearLastR = Environment.TickCount;
            }
            */
        }
    }
}
