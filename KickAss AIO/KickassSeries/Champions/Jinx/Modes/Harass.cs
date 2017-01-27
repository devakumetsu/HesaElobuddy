using System.Linq;
using EloBuddy;
using EloBuddy.SDK;

using Settings = KickassSeries.Champions.Jinx.Config.Modes.Harass;
using Misc = KickassSeries.Champions.Jinx.Config.Modes.Misc;

namespace KickassSeries.Champions.Jinx.Modes
{
    public sealed class Harass : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass);
        }

        public override void Execute()
        {/*
            #region Last Hitting Section

            // Force Minigun if there is a lasthittable minion in minigun range and there is no targets more than the setting amount.
            var kM = Orbwalker.LasthittableMinions.Where(
                t => t.IsEnemy &&
                     t.Health <= (Player.Instance.GetAutoAttackDamage(t)*0.9) && t.IsValidTarget() &&
                     t.Distance(Player.Instance) <= Essentials.MinigunRange);
            if (Settings.UseQ && Essentials.FishBones() && kM.Count() < 2 /*Settings.MinMinionQ)
            {
                Q.Cast();
            }

            // Out of Range
            if (Settings.UseQ && Player.Instance.ManaPercent >= 30/*Settings.ManaQ && !Essentials.FishBones())
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
                                100 && t.Health <= (Player.Instance.GetAutoAttackDamage(t)*1.1f)).ToArray();

                    if (minion.Count() >= Settings.MinMinionQ)
                    {
                        foreach (var m in minion)
                        {
                            Q.Cast();
                            Orbwalker.ForcedTarget = m;
                        }
                    }
                }
            }

            // In Range
            if (Settings.UseQ && Player.Instance.ManaPercent >= Settings.ManaQ && !Essentials.FishBones())
            {
                var minionInRange = Orbwalker.LasthittableMinions.FirstOrDefault(
                    m => m.IsValidTarget() && m.Distance(Player.Instance) <= Essentials.MinigunRange);

                if (minionInRange != null)
                {
                    var minion = EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy,
                        Player.Instance.ServerPosition,
                        Essentials.FishBonesRange())
                        .Where(
                            t =>
                                t.Distance(minionInRange
                                    ) <=
                                100 && t.Health <= (Player.Instance.GetAutoAttackDamage(t)*1.1f)).ToArray();

                    if (minion.Count() >= Settings.MinMinionQ)
                    {
                        foreach (var m in minion)
                        {
                            Q.Cast();
                            Orbwalker.ForcedTarget = m;
                        }
                    }
                }
            }

            #endregion

            #region Harassing Section

            // If the player has a minigun
            if (Settings.UseQ && Player.Instance.ManaPercent >= Settings.ManaQ && Q.IsReady() && !Essentials.FishBones())
            {
                var target = TargetSelector.GetTarget(Essentials.FishBonesRange(), DamageType.Physical);

                if (target != null && target.IsValidTarget())
                {
                    if (!Player.Instance.IsInAutoAttackRange(target) &&
                        Player.Instance.Distance(target) <= Essentials.FishBonesRange())
                    {
                        Q.Cast();
                        Orbwalker.ForcedTarget = target;
                    }

                    if (Player.Instance.IsInAutoAttackRange(target) &&
                        target.CountEnemiesInRange(100) >= Settings.MinChampQ)
                    {
                        Q.Cast();
                        Orbwalker.ForcedTarget = target;
                    }
                }
            }

            // If the player has the rocket
            if (Settings.UseQ && Q.IsReady() && Essentials.FishBones())
            {
                var target = TargetSelector.GetTarget(Essentials.FishBonesRange(), DamageType.Physical);

                if (target != null && target.IsValidTarget())
                {
                    if (Player.Instance.Distance(target) <= Essentials.MinigunRange &&
                        target.CountEnemiesInRange(100) < Settings.MinChampQ)
                    {
                        Q.Cast();
                        Orbwalker.ForcedTarget = target;
                    }
                }
            }

            if (Settings.UseW && Player.Instance.ManaPercent >= Settings.ManaW && W.IsReady())
            {
                var target = TargetSelector.GetTarget(W.Range, DamageType.Physical);

                if (target != null && target.IsValidTarget())
                {
                    if (Player.Instance.Distance(target) >= Settings.MinWRange)
                    {
                        var wPrediction = W.GetPrediction(target);

                        if (wPrediction.HitChancePercent >= Settings.PercentageW && !wPrediction.Collision)
                        {
                            if (Misc.WAARange && Player.Instance.IsInAutoAttackRange(target))
                            {
                                W.Cast(wPrediction.CastPosition);
                            }
                            else if (!Misc.WAARange)
                            {
                                W.Cast(wPrediction.CastPosition);
                            }
                        }
                    }
                }
            }

            #endregion
            */
        }
    }
}