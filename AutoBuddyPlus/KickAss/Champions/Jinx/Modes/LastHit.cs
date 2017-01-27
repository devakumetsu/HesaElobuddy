using System.Linq;
using EloBuddy;
using EloBuddy.SDK;

using Settings = KickassSeries.Champions.Jinx.Config.Modes.LastHit;

namespace KickassSeries.Champions.Jinx.Modes
{
    public sealed class LastHit : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LastHit);
        }

        public override void Execute()
        {/*
            // Force Minigun if there is a lasthittable minion in minigun range and there is no targets more than the setting amount.
            var kM = Orbwalker.LasthittableMinions.Where(
                t => t.IsEnemy &&
                     t.Health <= (Player.Instance.GetAutoAttackDamage(t) * 0.9) && t.IsValidTarget() &&
                     t.Distance(Player.Instance) <= Essentials.MinigunRange);
            if (Settings.UseQ && Essentials.FishBones() && kM.Count() < Settings.MinMinionsQ)
            {
                Q.Cast();
            }

            // Out of Range
            if (Settings.UseQ && Player.Instance.ManaPercent >= Settings.ManaQ && !Essentials.FishBones())
            {
                var minionOutOfRange = Orbwalker.LasthittableMinions.FirstOrDefault(
                    m =>
                        m.IsValidTarget() && m.Distance(Player.Instance) > Essentials.MinigunRange &&
                        m.Distance(Player.Instance) <= Essentials.FishBonesRange());

                if (minionOutOfRange != null)
                {
                    var minion = EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy,
                        Player.Instance.ServerPosition,
                        Essentials.FishBonesRange())
                        .Where(
                            t =>
                                t.Distance(minionOutOfRange
                                    ) <=
                                100 && t.Health <= (Player.Instance.GetAutoAttackDamage(t) * 1.1f)).ToArray();

                    if (minion.Count() >= 3/* Settings.MinMinionsQ)
                    {
                        foreach (var m in minion)
                        {
                            Q.Cast();
                            Orbwalker.ForcedTarget = m;
                        }
                    }
                }
            }*/
        }
    }
}
