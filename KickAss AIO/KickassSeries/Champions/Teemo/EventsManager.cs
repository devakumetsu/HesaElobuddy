using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu.Values;

using Combo = KickassSeries.Champions.Teemo.Config.Modes.Combo;
using Harass = KickassSeries.Champions.Teemo.Config.Modes.Harass;
using Misc = KickassSeries.Champions.Teemo.Config.Modes.Misc;

namespace KickassSeries.Champions.Teemo
{
    internal static class EventsManager
    {
        #region Spells

        private static Spell.Targeted Q
        {
            get { return SpellManager.Q; }
        }

        private static Spell.Active W
        {
            get { return SpellManager.W; }
        }

        private static Spell.Active E
        {
            get { return SpellManager.E; }
        }

        private static Spell.Skillshot R
        {
            get { return SpellManager.R; }
        }

        #endregion Spells

        public static int LastR;

        public static void Initialize()
        {
            Gapcloser.OnGapcloser += Gapcloser_OnGapcloser;
            Interrupter.OnInterruptableSpell += Interrupter_OnInterruptableSpell;
            Orbwalker.OnPostAttack += Orbwalker_OnPostAttack;
            Orbwalker.OnPreAttack += Orbwalker_OnPreAttack;
            Obj_AI_Base.OnProcessSpellCast += Obj_AI_Base_OnProcessSpellCast;
        }

        private static void Obj_AI_Base_OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!sender.IsMe)
            {
                return;
            }
            if (args.SData.Name.ToLower() == "teemorcast")
            {
                LastR = Environment.TickCount;
            }
        }

        private static void Orbwalker_OnPreAttack(AttackableUnit target, Orbwalker.PreAttackArgs args)
        {
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LastHit))
            {
                foreach (
                    var m in
                        ObjectManager.Get<Obj_AI_Base>()
                            .Where(
                                creep => creep.IsMinion && creep.IsEnemy && Player.Instance.IsInAutoAttackRange(creep))
                            .OrderBy(creep => creep.Health)
                            .Where(m => m != null)
                            .Where(
                                m =>
                                    m.Health <=
                                    Player.Instance.GetAutoAttackDamage(m) + SpellDamage.GetRealDamage(SpellSlot.E, m)))
                {
                    Orbwalker.DisableAttacking = true;
                    Orbwalker.DisableMovement = true;
                    Player.IssueOrder(GameObjectOrder.AttackUnit, m);
                    Orbwalker.DisableAttacking = false;
                    Orbwalker.DisableMovement = false;
                }
            }

            AIHeroClient enemy;
            Obj_AI_Base minion;

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass))
            {
                enemy =
                    EntityManager.Heroes.Enemies.Where(hero => Player.Instance.IsInAutoAttackRange(hero))
                        .OrderBy(hero => hero.Health)
                        .FirstOrDefault();
                minion =
                    ObjectManager.Get<Obj_AI_Base>()
                        .Where(unit => unit.IsMinion && unit.IsEnemy && Player.Instance.IsInAutoAttackRange(unit))
                        .OrderBy(unit => unit.Health)
                        .FirstOrDefault();

                #region Auto Attack

                if (minion == null)
                {
                    if (enemy != null)
                    {
                        if (Player.Instance.IsInAutoAttackRange(enemy))
                        {
                            Orbwalker.DisableAttacking = true;
                            Orbwalker.DisableMovement = true;
                            Player.IssueOrder(GameObjectOrder.AttackUnit, enemy);
                            Orbwalker.DisableAttacking = false;
                            Orbwalker.DisableMovement = false;
                        }
                    }
                }
                else
                {
                    if (enemy != null
                        &&
                        minion.Health >
                        Player.Instance.GetAutoAttackDamage(minion) + SpellDamage.GetRealDamage(SpellSlot.E, minion))
                    {
                        if (Player.Instance.IsInAutoAttackRange(enemy))
                        {
                            Orbwalker.DisableAttacking = true;
                            Orbwalker.DisableMovement = true;
                            Player.IssueOrder(GameObjectOrder.AttackUnit, enemy);
                            Orbwalker.DisableAttacking = false;
                            Orbwalker.DisableMovement = false;
                        }
                    }
                    else if (minion.Health <=
                             Player.Instance.GetAutoAttackDamage(minion) + SpellDamage.GetRealDamage(SpellSlot.E, minion))
                    {
                        Orbwalker.DisableAttacking = true;
                        Orbwalker.DisableMovement = true;
                        Player.IssueOrder(GameObjectOrder.AttackUnit, minion);
                        Orbwalker.DisableAttacking = false;
                        Orbwalker.DisableMovement = false;
                    }
                }

                #endregion
            }

            if (!Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear))
            {
                return;
            }
            enemy =
                EntityManager.Heroes.Enemies.Where(hero => Player.Instance.IsInAutoAttackRange(hero))
                    .OrderBy(hero => hero.Health)
                    .FirstOrDefault();
            minion =
                ObjectManager.Get<Obj_AI_Base>()
                    .Where(unit => unit.IsMinion && unit.IsEnemy && Player.Instance.IsInAutoAttackRange(unit))
                    .OrderBy(unit => unit.Health)
                    .FirstOrDefault();

            if (minion == null)
            {
                if (enemy == null)
                {
                    return;
                }
                if (!Player.Instance.IsInAutoAttackRange(enemy))
                {
                    return;
                }
                Orbwalker.DisableAttacking = true;
                Orbwalker.DisableMovement = true;
                Player.IssueOrder(GameObjectOrder.AttackUnit, enemy);
                Orbwalker.DisableAttacking = false;
                Orbwalker.DisableMovement = false;
            }
            else
            {
                if (enemy != null
                    &&
                    minion.Health >
                    Player.Instance.GetAutoAttackDamage(minion) + SpellDamage.GetRealDamage(SpellSlot.E, minion))
                {
                    if (!Player.Instance.IsInAutoAttackRange(enemy))
                    {
                        return;
                    }
                    Orbwalker.DisableAttacking = true;
                    Orbwalker.DisableMovement = true;
                    Player.IssueOrder(GameObjectOrder.AttackUnit, enemy);
                    Orbwalker.DisableAttacking = false;
                    Orbwalker.DisableMovement = false;
                }
                else if (minion.Health <=
                         Player.Instance.GetAutoAttackDamage(minion) + SpellDamage.GetRealDamage(SpellSlot.E, minion))
                {
                    Orbwalker.DisableAttacking = true;
                    Orbwalker.DisableMovement = true;
                    Player.IssueOrder(GameObjectOrder.AttackUnit, minion);
                    Orbwalker.DisableAttacking = false;
                    Orbwalker.DisableMovement = false;
                }
            }
        }

        private static void Orbwalker_OnPostAttack(AttackableUnit target, System.EventArgs args)
        {
            var t = target as AIHeroClient;

            if (t != null && Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
            {/*
                #region Check AA

                
                if (Misc.CheckAA)
                {
                    if (Combo.OnlyQADC)
                    {
                        foreach (var adc in Teemo.Marksman)
                        {
                            if (t.Name == adc && Combo.UseQ && Q.IsReady() && Player.Instance.Distance(target) < Q.Range - Misc.CheckAArange)
                            {
                                Q.Cast(t);
                            }
                            else
                            {
                                return;
                            }
                        }
                    }
                    else
                    {
                        if (Combo.UseQ && Q.IsReady() && Player.Instance.Distance(target) < Q.Range - Misc.CheckAArange)
                        {
                            Q.Cast(t);
                        }
                        else
                        {
                            return;
                        }
                    }
                }
                

                #endregion

                #region No Check AA

                else
                {
                    /*
                    if (Combo.OnlyQADC)
                    {
                        foreach (var adc in Teemo.Marksman)
                        {
                            if (t.Name == adc && Combo.UseQ && Q.IsReady() && Q.IsInRange(t))
                            {
                                Q.Cast(t);
                            }
                            else
                            {
                                return;
                            }
                        }
                    }
                    
                    else
                    {
                        if (Combo.UseQ && Q.IsReady() && Q.IsInRange(t))
                        {
                            Q.Cast(t);
                        }
                        else
                        {
                            return;
                        }
                    }
                }

                #endregion
            }
            if (t == null || !Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass))
            {
                return;
            }
            /*
            if (Misc.CheckAA)
            {
                if (Harass.UseQ && Q.IsReady() && Player.Instance.Distance(t) < Q.Range - Misc.CheckAArange)
                {
                    Q.Cast(t);
                }
            }
            else
            {
                if (Harass.UseQ && Q.IsReady() && Player.Instance.Distance(t) < Q.Range)
                {
                    Q.Cast(t);
                }*/
            }

        }

        private static void Gapcloser_OnGapcloser(AIHeroClient sender, Gapcloser.GapcloserEventArgs e)
        {
            if (!sender.IsEnemy) return;

            if (sender.IsValidTarget(SpellManager.E.Range))
            {
                SpellManager.E.Cast(sender);
            }
        }

        private static void Interrupter_OnInterruptableSpell(Obj_AI_Base sender,
            Interrupter.InterruptableSpellEventArgs e)
        {
            if (!sender.IsEnemy) return;

            if (e.DangerLevel == DangerLevel.High)
            {
                SpellManager.E.Cast(sender);
            }
        }
    }
}