using System.Linq;
using EloBuddy;
using EloBuddy.SDK;

using Settings = KickassSeries.Champions.Jinx.Config.Modes.Combo;
using Misc = KickassSeries.Champions.Jinx.Config.Modes.Misc;

namespace KickassSeries.Champions.Jinx.Modes
{
    public sealed class Combo : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo);
        }

        public override void Execute()
        {/*
            if (Settings.UseQ && Player.Instance.ManaPercent >= Settings.ManaQ && SpellManager.Q.IsReady() && !Essentials.FishBones())
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
                        target.CountEnemiesInRange(100) >= Settings.QCount)
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
                    if (Player.Instance.Distance(target) >= Settings.WRange)
                    {
                        var wPrediction = W.GetPrediction(target);

                        if (wPrediction != null && !wPrediction.Collision && wPrediction.HitChancePercent >= Settings.WPrediction)
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

            if (Settings.UseE && Player.Instance.ManaPercent >= Settings.ManaE && E.IsReady())
            {
                var target = TargetSelector.GetTarget(Settings.ERange2,DamageType.Physical);

                if (target != null)
                {
                    if (Player.Instance.Distance(target) <= Settings.ERange)
                    {
                        var ePrediction = E.GetPrediction(target);

                        if (ePrediction != null && ePrediction.HitChancePercent >= Settings.EPrediction &&
                            !target.IsFacing(Player.Instance))
                        {
                            E.Cast(ePrediction.CastPosition);
                        }
                    }
                }
            }

            if (Settings.UseR && Player.Instance.ManaPercent >= Settings.ManaR && R.IsReady())
            {
                var target = TargetSelector.GetTarget(Settings.RRange, DamageType.Physical);

                if (target != null)
                {
                    if (Player.Instance.Distance(target) >= Misc.RRange)
                    {
                        var rPrediction = R.GetPrediction(target);

                        if (rPrediction != null && rPrediction.HitChancePercent >= Settings.RPrediction &&
                            EntityManager.Heroes.Enemies.Count(
                                t => t.IsValidTarget() && t.Distance(rPrediction.CastPosition) <= 200) >= Settings.RCount)
                        {
                            R.Cast(rPrediction.CastPosition);
                        }
                    }
                }
            }*/
        }
    }
}
