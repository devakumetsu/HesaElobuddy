using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;

using Misc = KickassSeries.Champions.Jinx.Config.Modes.Misc;

namespace KickassSeries.Champions.Jinx.Modes
{
    public sealed class PermaActive : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return true;
        }

        public override void Execute()
        {
            /*
            if (Player.Instance.IsDead || Player.Instance.HasBuff("Recall")
                || Player.Instance.IsStunned || Player.Instance.IsRooted || Player.Instance.IsCharmed ||
                Orbwalker.IsAutoAttacking)
            {
                return;
            }

            if (KS.ToggleKillSteal)
            {
                WR_KillSteal();
                StandaloneKillSteal();
            }

            if (JS.JungleStealToggle)
            {
                JungleSteal();
            }

            if (Misc.AutoW)
            {
                AutoW();
            }

            if (Misc.AutoE)
            {
                AutoE();
            }
        }

        private static void AutoW()
        {
            if (!W.IsLearned || !W.IsReady() || Orbwalker.IsAutoAttacking ||
                EntityManager.Turrets.Enemies.Count(t => t.IsValidTarget() && t.IsAttackingPlayer) > 0)
            {
                return;
            }

            var enemy =
                EntityManager.Heroes.Enemies.Where(t => t.IsValidTarget() && W.IsInRange(t))
                    .OrderByDescending(t => t.Distance(Player.Instance));

            foreach (var target in enemy)
            {
                if (Misc.WAARange && Player.Instance.Distance(target) <= Player.Instance.GetAutoAttackRange())
                {
                    return;
                }

                if (Misc.StunW && target.IsStunned)
                {
                    var prediction = W.GetPrediction(target);

                    if (prediction.HitChancePercent >= Misc.WPredSlider && !prediction.Collision)
                    {
                        W.Cast(prediction.CastPosition);
                    }
                }

                else if (Misc.CharmW && target.IsCharmed)
                {
                    var prediction = W.GetPrediction(target);

                    if (prediction.HitChancePercent >= Misc.WPredSlider && !prediction.Collision)
                    {
                        W.Cast(prediction.CastPosition);
                    }
                }

                else if (Misc.TauntW && target.IsTaunted)
                {
                    var prediction = W.GetPrediction(target);

                    if (prediction.HitChancePercent >= Misc.WPredSlider && !prediction.Collision)
                    {
                        W.Cast(prediction.CastPosition);
                    }
                }

                else if (Misc.FearW && target.IsFeared)
                {
                    var prediction = W.GetPrediction(target);

                    if (prediction.HitChancePercent >= Misc.WPredSlider && !prediction.Collision)
                    {
                        W.Cast(prediction.CastPosition);
                    }
                }

                else if (Misc.SnareW && target.IsRooted)
                {
                    var prediction = W.GetPrediction(target);

                    if (prediction.HitChancePercent >= Misc.WPredSlider && !prediction.Collision)
                    {
                        W.Cast(prediction.CastPosition);
                    }
                }
            }
        }

        private static void AutoE()
        {
            if (!E.IsLearned || !E.IsReady())
            {
                return;
            }

            foreach (var pred in EntityManager.Heroes.Enemies.Where(
                enemy => enemy != null &&
                         E.IsInRange(enemy) && enemy.IsValidTarget() && !enemy.CanMove &&
                         Game.Time - Essentials.GrabTime > 1)
                .Select(enemy => E.GetPrediction(enemy))
                .Where(pred => pred != null && pred.HitChancePercent >= 75))
            {
                E.Cast(pred.CastPosition);
            }
        }

        private static void WR_KillSteal()
        {
            if (KS.UseW && KS.UseR && Player.Instance.ManaPercent >= KS.WMana && Player.Instance.ManaPercent >= KS.RMana
                && W.IsReady() && R.IsReady())
            {
                var selection =
                    EntityManager.Heroes.Enemies.Where(
                        t =>
                            t.IsValidTarget() && W.IsInRange(t) &&
                            Player.Instance.Distance(t) <=
                            KS.RMaxRange &&
                            Player.Instance.Distance(t) >= Misc.RRange
                            && Essentials.DamageLibrary.CalculateDamage(t, false, true, false, true) >= t.Health);

                foreach (var enemy in selection)
                {
                    var pred = W.GetPrediction(enemy);

                    if (pred != null && pred.HitChancePercent >= KS.WPredSlider && !pred.Collision)
                    {
                        W.Cast(pred.CastPosition);
                        var target = enemy;

                        Core.DelayAction(() =>
                        {
                            var predR = R.GetPrediction(target);
                            var checkDmg = target.Health <=
                                           Essentials.DamageLibrary.CalculateDamage(target, false, false, false, true);

                            if (predR != null && predR.HitChancePercent >= KS.RPredSlider && checkDmg)
                            {
                                R.Cast(predR.CastPosition);
                            }
                        }, W.CastDelay);
                    }
                }
            }
        }

        private static void StandaloneKillSteal()
        {
            if (KS.UseW && Player.Instance.ManaPercent >= KS.WMana && W.IsReady())
            {
                var selection =
                    EntityManager.Heroes.Enemies.Where(
                        t =>
                            t.IsValidTarget() && W.IsInRange(t)
                            && Essentials.DamageLibrary.CalculateDamage(t, false, true, false, false) >= t.Health);

                foreach (
                    var pred in
                        selection.Select(enemy => W.GetPrediction(enemy))
                            .Where(pred => pred != null && pred.HitChancePercent >= KS.WPredSlider && !pred.Collision))
                {
                    W.Cast(pred.CastPosition);
                }
            }

            if (KS.UseR && Player.Instance.ManaPercent >= KS.RMana && R.IsReady())
            {
                var selection =
                    EntityManager.Heroes.Enemies.Where(
                        t =>
                            t.IsValidTarget()
                            &&
                            Player.Instance.Distance(t) <=
                            KS.RMaxRange &&
                            Player.Instance.Distance(t) >= Misc.RRange
                            && Essentials.DamageLibrary.CalculateDamage(t, false, false, false, true) >= t.Health);

                foreach (
                    var pred in
                        selection.Select(enemy => R.GetPrediction(enemy))
                            .Where(pred => pred != null && pred.HitChancePercent >= KS.RPredSlider))
                {
                    R.Cast(pred.CastPosition);
                }
            }
        }

        private static void JungleSteal()
        {
            if (Player.Instance.ManaPercent >= JS.ManaR)
            {
                switch (Game.MapId)
                {
                    case GameMapId.SummonersRift:
                        var jungleMobSR =
                            EntityManager.MinionsAndMonsters.Monsters.FirstOrDefault(
                                u =>
                                    u.IsVisible && Essentials.JungleMobsList.Contains(u.BaseSkinName) &&
                                    Essentials.DamageLibrary.CalculateDamage(u, false, false, false, true) >= u.Health);

                        if (jungleMobSR == null)
                        {
                            return;
                        }

                        if (
                            !Config.Modes.JungleStealMenu[jungleMobSR.BaseSkinName + "jinx"].Cast<CheckBox>()
                                .CurrentValue)
                        {
                            return;
                        }

                        var enemySR =
                            EntityManager.Heroes.Enemies.Where(t => t.Distance(jungleMobSR) <= 100)
                                .OrderByDescending(t => t.Distance(jungleMobSR));

                        if (enemySR.Any())
                        {
                            foreach (var target in enemySR.Where(target => Player.Instance.Distance(target) < JS.RangeR)
                                )
                            {
                                if (target.Distance(jungleMobSR) <= 100)
                                {
                                    R.Cast(jungleMobSR.ServerPosition);
                                }
                            }
                        }
                        break;
                    case GameMapId.TwistedTreeline:
                        var jungleMobTT =
                            EntityManager.MinionsAndMonsters.Monsters.FirstOrDefault(
                                u =>
                                    u.IsVisible && Essentials.JungleMobsListTwistedTreeline.Contains(u.BaseSkinName) &&
                                    Essentials.DamageLibrary.CalculateDamage(u, false, false, false, true) >= u.Health);

                        if (jungleMobTT == null)
                        {
                            return;
                        }

                        if (
                            !Config.Modes.JungleStealMenu[jungleMobTT.BaseSkinName + "jinx"].Cast<CheckBox>()
                                .CurrentValue)
                        {
                            return;
                        }

                        var enemy =
                            EntityManager.Heroes.Enemies.Where(t => t.Distance(jungleMobTT) <= 100)
                                .OrderByDescending(t => t.Distance(jungleMobTT));

                        if (enemy.Any())
                        {
                            foreach (var target in enemy.Where(target => Player.Instance.Distance(target) < JS.RangeR))
                            {
                                if (target.Distance(jungleMobTT) <= 100)
                                {
                                    R.Cast(jungleMobTT.ServerPosition);
                                }
                            }
                        }
                        break;
                }
            }*/
        }
    }
}
