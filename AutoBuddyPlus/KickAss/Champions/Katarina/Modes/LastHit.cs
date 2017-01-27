using System.Linq;
using EloBuddy;
using EloBuddy.SDK;

using Settings = KickassSeries.Champions.Katarina.Config.Modes.LastHit;

namespace KickassSeries.Champions.Katarina.Modes
{
    public sealed class LastHit : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LastHit);
        }

        public override void Execute()
        {
            var minion =
                EntityManager.MinionsAndMonsters.GetLaneMinions()
                    .OrderByDescending(m => m.Health)
                    .FirstOrDefault(m => m.IsValidTarget(Q.Range));

            if (minion == null) return;


            //TODO TEST THIS CODE
            if (E.IsReady() && minion.IsValidTarget(E.Range) && Settings.UseE)
            {
                var minions =
                    EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy,
                        Player.Instance.ServerPosition, E.Range, false).Where(m => m.Health < SpellDamage.GetRealDamage(SpellSlot.W, m)).ToArray();
                if (minions.Length != 0)
                {
                    var farmLocation = EntityManager.MinionsAndMonsters.GetCircularFarmLocation(minions, W.Range,
                        (int) E.Range);

                    if (farmLocation.HitNumber >= 2)
                    {
                        E.Cast(farmLocation.CastPosition);
                    }
                }
            }

            if (W.IsReady() && minion.IsValidTarget(W.Range) && Settings.UseW)
            {
                var minionW =
                    EntityManager.MinionsAndMonsters.GetLaneMinions()
                        .OrderByDescending(m => m.Health)
                        .FirstOrDefault(
                            m => m.IsValidTarget(E.Range) && m.Health <= SpellDamage.GetRealDamage(SpellSlot.W, m));
                if (minionW != null)
                {
                    W.Cast();
                }
            }

            if (Q.IsReady() && minion.IsValidTarget(Q.Range) && Settings.UseQ)
            {
                var minionQ =
                    EntityManager.MinionsAndMonsters.GetLaneMinions()
                        .OrderByDescending(m => m.Health)
                        .FirstOrDefault(
                            m => m.IsValidTarget(E.Range) && m.Health <= SpellDamage.GetRealDamage(SpellSlot.Q, m));
                if (minionQ != null)
                {
                    Q.Cast(minionQ);
                }
            }
        }
    }
}
