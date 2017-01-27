using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu.Values;

using Misc = KickassSeries.Champions.Jinx.Config.Modes.Misc;
using Combo = KickassSeries.Champions.Jinx.Config.Modes.Combo;
using LastHit = KickassSeries.Champions.Jinx.Config.Modes.LastHit;
using LaneClear = KickassSeries.Champions.Jinx.Config.Modes.LaneClear;

namespace KickassSeries.Champions.Jinx
{
    internal static class EventsManager
    {
        
        public static void Initialize()
        {/*
            Gapcloser.OnGapcloser += Gapcloser_OnGapcloser;
            Interrupter.OnInterruptableSpell += Interrupter_OnInterruptableSpell;
            Orbwalker.OnPreAttack += Orbwalker_OnPreAttack;
            Orbwalker.OnPostAttack += Orbwalker_OnPostAttack;
            Obj_AI_Base.OnProcessSpellCast += Obj_AI_Base_OnProcessSpellCast;
            */
        }
        /*
        private static void Obj_AI_Base_OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!Misc.AutoE || !SpellManager.E.IsReady())
            {
                return;
            }

            if (sender.IsEnemy && sender.IsValidTarget(SpellManager.E.Range) && Essentials.ShouldUseE(args.SData.Name))
            {
                var prediction = SpellManager.E.GetPrediction(sender);

                if (prediction.HitChancePercent >= Misc.EPredSlider)
                {
                    SpellManager.E.Cast(prediction.CastPosition);
                }
            }

            if (sender.IsAlly && args.SData.Name == "RocketGrab" && SpellManager.E.IsInRange(sender))
            {
                Essentials.GrabTime = Game.Time;
            }
        }

        private static void Orbwalker_OnPostAttack(AttackableUnit target, System.EventArgs args)
        {
            Orbwalker.ForcedTarget = null;
        }

        private static void Orbwalker_OnPreAttack(AttackableUnit target, Orbwalker.PreAttackArgs args)
        {
            #region Combo

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
            {
                // If the player has the rocket
                if (Combo.UseQ && SpellManager.Q.IsReady() && Essentials.FishBones())
                {
                    //var target = TargetSelector.GetTarget(Essentials.FishBonesRange(), DamageType.Physical);

                    if (target != null && target.IsValidTarget())
                    {
                        if (Player.Instance.Distance(target) <= Essentials.MinigunRange &&
                            target.CountEnemiesInRange(100) <
                            Combo.QCount)
                        {
                            SpellManager.Q.Cast();
                            Orbwalker.ForcedTarget = target;
                        }
                    }
                }
            }

            #endregion

            #region LastHit

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LastHit))
            {
                // In Range
                if (LastHit.UseQ && Player.Instance.ManaPercent >= LastHit.ManaQ && !Essentials.FishBones())
                {
                    var minionInRange = target as Obj_AI_Minion;
                    //Orbwalker.LasthittableMinions.FirstOrDefault(m => m.IsValidTarget() && m.Distance(Player.Instance) <= Essentials.MinigunRange);

                    if (minionInRange != null)
                    {
                        var minion = EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy,
                            Player.Instance.ServerPosition,
                            Essentials.FishBonesRange())
                            .Where(
                                t =>
                                    t.Distance(minionInRange
                                        ) <=
                                    100 && t.Health <= (Player.Instance.GetAutoAttackDamage(t) * 1.1f)).ToArray();

                        if (minion.Count() >= LastHit.MinMinionsQ)
                        {
                            foreach (var m in minion)
                            {
                                SpellManager.Q.Cast();
                                Orbwalker.ForcedTarget = m;
                            }
                        }
                    }
                }
            }

            #endregion

            #region Lane Clear

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear))
            {
                var minion = EntityManager.MinionsAndMonsters.GetLaneMinions(
                    EntityManager.UnitTeam.Enemy,
                    Player.Instance.ServerPosition,
                    Essentials.FishBonesRange()).OrderByDescending(t => t.Health);

                if (Essentials.FishBones())
                {
                    foreach (var m in minion)
                    {
                        var minionsAoe =
                            EntityManager.MinionsAndMonsters.EnemyMinions.Count(
                                t => t.IsValidTarget() && t.Distance(m) <= 100);

                        if (m.Distance(Player.Instance) <= Essentials.MinigunRange && m.IsValidTarget() &&
                            (minionsAoe < LaneClear.MinMinionLane ||
                             m.Health > (Player.Instance.GetAutoAttackDamage(m))))
                        {
                            SpellManager.Q.Cast();
                            Orbwalker.ForcedTarget = m;
                        }
                        else if (m.Distance(Player.Instance) <= Essentials.MinigunRange &&
                                 !Orbwalker.LasthittableMinions.Contains(m))
                        {
                            SpellManager.Q.Cast();
                            Orbwalker.ForcedTarget = m;
                        }
                        else
                        {
                            foreach (
                                var kM in
                                    Orbwalker.LasthittableMinions.Where(
                                        kM =>
                                            kM.IsValidTarget() &&
                                            kM.Health <= (Player.Instance.GetAutoAttackDamage(kM) * 0.9) &&
                                            kM.Distance(Player.Instance) <= Essentials.MinigunRange))
                            {
                                SpellManager.Q.Cast();
                                Orbwalker.ForcedTarget = kM;
                            }
                        }
                    }
                }
            }

            #endregion

            if (Essentials.FishBones() && target.IsStructure() &&
                target.Distance(Player.Instance) <= Essentials.MinigunRange)
            {
                SpellManager.Q.Cast();
            }
        }

        private static void Gapcloser_OnGapcloser(AIHeroClient sender, Gapcloser.GapcloserEventArgs e)
        {
            if (sender != null && sender.IsEnemy && Misc.GapCloserE
                && Player.Instance.ManaPercent >= Misc.GapCloserEMana
                && (SpellManager.E.IsInRange(sender) && SpellManager.E.IsReady() && sender.IsValidTarget()))
            {
                var pred = SpellManager.E.GetPrediction(sender);

                if (pred != null && pred.HitChancePercent >= 75)
                {
                    SpellManager.E.Cast(pred.CastPosition);
                }
            }
        }

        private static void Interrupter_OnInterruptableSpell(Obj_AI_Base sender, Interrupter.InterruptableSpellEventArgs e)
        {
            if (sender != null && !sender.IsAlly && Misc.InterruptE
                && Player.Instance.ManaPercent >= Misc.InterruptEMana
                && (SpellManager.E.IsInRange(sender) && SpellManager.E.IsReady() && sender.IsValidTarget() && e.DangerLevel == DangerLevel.High))
            {
                var pred = SpellManager.E.GetPrediction(sender);

                if (pred != null && pred.HitChancePercent >= 75)
                {
                    SpellManager.E.Cast(pred.CastPosition);
                }
            }
        }*/
    }
}
