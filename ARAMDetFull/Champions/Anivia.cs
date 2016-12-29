using System;
using System.Collections.Generic;
using System.Linq;
using SharpDX;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;
using static EloBuddy.SDK.Spell;

namespace ARAMDetFull.Champions
{
    class Anivia : Champion
    {

        public static int FarmId;

        public static GameObject QMissile;
        public static GameObject RMissile;

        public Anivia()
        {
            ARAMSimulator.champBuild = new Build
            {
                coreItems = new List<ConditionalItem>
                {
                    new ConditionalItem(ItemId.Rod_of_Ages),
                    new ConditionalItem(ItemId.Sorcerers_Shoes),
                    new ConditionalItem(ItemId.Morellonomicon),
                    new ConditionalItem(ItemId.Liandrys_Torment),
                    new ConditionalItem(ItemId.Zhonyas_Hourglass),
                    new ConditionalItem(ItemId.Void_Staff),
                },
                startingItems = new List<ItemId>
                {
                    ItemId.Catalyst_of_Aeons
                }
            };
            GameObject.OnDelete += Obj_AI_Base_OnDelete;
            GameObject.OnCreate += Obj_AI_Base_OnCreate;

            Orbwalker.OnPreAttack += Orbwalker_OnPreAttack;
            Gapcloser.OnGapcloser += Gapcloser_OnGapcloser;
            Interrupter.OnInterruptableSpell += Interrupter_OnInterruptableSpell;
        }

        private void Gapcloser_OnGapcloser(AIHeroClient Target, Gapcloser.GapcloserEventArgs e)
        {
            if (Target == null && !Target.IsEnemy && Target.IsDead) return;
            if (Q.IsReady())
            {
                if (Target.IsValidTarget(Q.Range))
                {
                    Q.Cast(Target);
                }
            }
            else if (W.IsReady())
            {
                if (Target.IsValidTarget(W.Range))
                {
                    W.Cast(Target);
                }
            }
        }

        private void Interrupter_OnInterruptableSpell(Obj_AI_Base unit, Interrupter.InterruptableSpellEventArgs e)
        {
            if (unit == null && !unit.IsEnemy && unit.IsDead) return;
            if (W.IsReady() && unit.IsValidTarget(W.Range))
                W.Cast(unit);
        }

        private void Orbwalker_OnPreAttack(AttackableUnit target, Orbwalker.PreAttackArgs args)
        {
            if (target == null && !target.IsEnemy && target.IsDead) return;
            if (FarmId != args.Target.NetworkId)
                FarmId = args.Target.NetworkId;
        }

        private void Obj_AI_Base_OnCreate(GameObject obj, EventArgs args)
        {
            if (obj.IsValid)
            {
                if (obj.Name == "cryo_FlashFrost_Player_mis.troy")
                    QMissile = obj;
                if (obj.Name.Contains("cryo_storm"))
                    RMissile = obj;
            }
        }

        private void Obj_AI_Base_OnDelete(GameObject obj, EventArgs args)
        {
            if (obj.IsValid)
            {
                if (obj.Name == "cryo_FlashFrost_Player_mis.troy")
                    QMissile = null;
                if (obj.Name.Contains("cryo_storm"))
                    RMissile = null;
            }
        }


        public override void useQ(Obj_AI_Base target)
        {
        }

        public override void useW(Obj_AI_Base target)
        {
        }

        public override void useE(Obj_AI_Base target)
        {
        }


        public override void useR(Obj_AI_Base target)
        {
            if (!R.IsReady() || target == null)
                return;
            if (target.HealthPercent < 35)
                R.Cast(target);
        }

        public override void useSpells()
        {
            if (R.IsReady())
            {
                var t = TargetSelector.GetTarget(R.Range + 400, DamageType.Physical);
                if (RMissile == null && t.IsValidTarget())
                {

                    R.Cast(t);
                }
                var allMinionsQ = EntityManager.MinionsAndMonsters.EnemyMinions.Where(x => x.IsInRange(Player.Instance.ServerPosition, R.Range + 400));
                var Rfarm = R.GetCircularFarmLocation(allMinionsQ, R.Width());

                if (RMissile == null
                    && Farm
                    && Rfarm.HitNumber > 2)
                {
                    R.Cast(Rfarm.CastPosition);
                }

                if (Combo && RMissile != null && (RMissile.Position.CountEnemyHeroesInRangeWithPrediction(450) == 0))
                {
                    R.Cast();
                }
                else if (RMissile != null && Farm && (Rfarm.HitNumber < 3 || Rfarm.CastPosition.Distance(RMissile.Position) > 400))
                {
                    R.Cast();
                }
                if (!Combo && !Farm && RMissile != null)
                    R.Cast();
            }
            if (W.IsReady())
            {
                var ta = ARAMTargetSelector.getBestTarget(W.Range);
                if (Combo && ta.IsValidTarget(W.Range) && ta.Path.Count() == 1 && W.GetPrediction(ta).CastPosition.Distance(ta.Position) > 150)
                {
                    if (player.Position.Distance(ta.ServerPosition) > player.Position.Distance(ta.Position))
                    {
                        if (ta.Position.Distance(player.ServerPosition) < ta.Position.Distance(player.Position) && ta.IsValidTarget(W.Range - 200))
                            CastSpell(W, ta, 3);
                    }
                    else
                    {
                        if (ta.Position.Distance(player.ServerPosition) > ta.Position.Distance(player.Position) && ta.IsValidTarget(E.Range) && ta.HasBuffOfType(BuffType.Slow))
                            CastSpell(W, ta, 3);
                    }
                }
            }
            /*
            if (Q.IsReady() && QMissile == null)
            {
                var t = ARAMTargetSelector.getBestTarget(Q.Range);
                if (t.IsValidTarget())
                {
                    var qDmg = Q.GetDamage(t);

                    if (qDmg > t.Health)
                        Q.Cast(t);
                    else if (Combo)
                        CastSpell(Q, t, 3);
                    else if (Farm && !player.IsUnderEnemyturret())
                    {
                        foreach (var enemy in ObjectManager.Get<AIHeroClient>().Where(enemy => enemy.IsValidTarget(Q.Range)))
                        {
                            CastSpell(Q, enemy, 3);
                        }
                    }

                    else
                    {
                        foreach (var enemy in ObjectManager.Get<AIHeroClient>().Where(enemy => enemy.IsValidTarget(Q.Range)))
                        {
                            if (enemy.HasBuffOfType(BuffType.Stun) || enemy.HasBuffOfType(BuffType.Snare) ||
                             enemy.HasBuffOfType(BuffType.Charm) || enemy.HasBuffOfType(BuffType.Fear) ||
                             enemy.HasBuffOfType(BuffType.Taunt) || enemy.HasBuffOfType(BuffType.Slow) || enemy.HasBuff("Recall"))
                            {
                                Q.Cast(enemy);
                            }
                        }
                    }
                }
            }*/

            if (E.IsReady())
            {
                var t = ARAMTargetSelector.getBestTarget(E.Range);
                if (t.IsValidTarget())
                {
                    var qCd = Q.CooldownExpires() - Game.Time;
                    var rCd = R.CooldownExpires() - Game.Time;
                    if (player.Level < 7)
                        rCd = 10;
                    //debug("Q " + qCd + "R " + rCd + "E now " + E.Instance.Cooldown);
                    var eDmg = E.GetDamage(t);
                    if (t.HasBuff("chilled"))
                    {
                        eDmg = 2 * eDmg;
                    }
                    if (eDmg > t.Health)
                        E.Cast(t);
                    else if ((t.HasBuff("chilled")) && Combo && QMissile == null)
                    {
                        if (RMissile == null && R.IsReady())
                            R.Cast(t);
                        E.Cast(t);
                    }
                    else if (t.HasBuff("chilled") && Farm && !player.IsUnderEnemyturret() && QMissile == null)
                    {
                        if (RMissile == null && R.IsReady())
                            R.Cast(t);
                        E.Cast(t);
                    }
                    else if (t.HasBuff("chilled") && Combo)
                    {
                        E.Cast(t);
                    }
                }
                farmE();
            }
            /*
            if (Q.IsReady() && QMissile != null)
            {
                if (QMissile.Position.CountEnemyHeroesInRangeWithPrediction(220) > 0)
                    Q.Cast();
            }*/
        }

        public void farmE()
        {
            if (Farm && !Orbwalker.CanAutoAttack)
            {

                var mobs = EntityManager.MinionsAndMonsters.Monsters.Where(x => x.IsInRange(Player.Instance.ServerPosition, E.Range)).OrderBy(x => x.MaxHealth).ToList();
                if (mobs.Count() > 0)
                {
                    var mob = mobs[0];
                    E.Cast(mob);
                    return;
                }

                var minions = EntityManager.MinionsAndMonsters.EnemyMinions.Where(x => x.IsInRange(Player.Instance.ServerPosition, E.Range)).OrderBy(x => x.MaxHealth).ToList();
                foreach (var minion in minions.Where(minion => minion.Health > player.GetAutoAttackDamage(minion) && FarmId != minion.NetworkId))
                {
                    var eDmg = E.GetDamage(minion);
                    if (minion.HasBuff("chilled"))
                        eDmg = 2 * eDmg;

                    if (minion.Health < eDmg * 0.9)
                        E.Cast(minion);
                }
            }
        }

        private void CastSpell(SpellBase QWER, AIHeroClient target, int HitChanceNum)
        {
            QWER.CastIfHitchanceEquals(target, (EloBuddy.SDK.Enumerations.HitChance)HitChanceNum, true);
            return;



            //HitChance 0 - 2
            // example CastSpell(Q, ts, 2);
            var poutput = QWER.GetPrediction(target);
            var col = poutput.CollisionObjects.Count(ColObj => ColObj.IsEnemy && ColObj.IsMinion && !ColObj.IsDead);
            if (target.IsDead || col > 0 || target.Path.Count() > 1)
                return;

            if ((target.Path.Count() == 0 && target.Position == target.ServerPosition) || target.HasBuff("Recall"))
            {
                QWER.Cast(poutput.CastPosition);
                return;
            }

            if (HitChanceNum == 0)
                QWER.Cast(target);
            else if (HitChanceNum == 1)
            {
                if ((int)poutput.HitChance > 4)
                    QWER.Cast(poutput.CastPosition);
            }
            else if (HitChanceNum == 2)
            {
                List<Vector2> waypoints = target.GetWaypoints();
                if (waypoints.Last().To3D().Distance(poutput.CastPosition) > QWER.Width() && (int)poutput.HitChance == 5)
                {
                    if (waypoints.Last().To3D().Distance(player.Position) <= target.Distance(player.Position) || (target.Path.Count() == 0 && target.Position == target.ServerPosition))
                    {
                        if (player.Distance(target.ServerPosition) < QWER.Range - (poutput.CastPosition.Distance(target.ServerPosition) + target.BoundingRadius))
                        {
                            QWER.Cast(poutput.CastPosition);
                        }
                    }
                    else if ((int)poutput.HitChance == 5)
                    {
                        QWER.Cast(poutput.CastPosition);
                    }
                }
            }
            else if (HitChanceNum == 3)
            {
                List<Vector2> waypoints = target.GetWaypoints();
                float SiteToSite = ((target.MoveSpeed * QWER.CastDelay) + (player.Distance(target.ServerPosition) / QWER.Speed()) - QWER.Width()) * 6;
                float BackToFront = ((target.MoveSpeed * QWER.CastDelay) + (player.Distance(target.ServerPosition) / QWER.Speed()));
                if (player.Distance(waypoints.Last().To3D()) < SiteToSite || player.Distance(target.Position) < SiteToSite)
                    QWER.CastIfHitchanceEquals(target, EloBuddy.SDK.Enumerations.HitChance.High, true);
                else if ((target.ServerPosition.Distance(waypoints.Last().To3D()) > SiteToSite
                    || Math.Abs(player.Distance(waypoints.Last().To3D()) - player.Distance(target.Position)) > BackToFront))
                {
                    if (waypoints.Last().To3D().Distance(player.Position) <= target.Distance(player.Position))
                    {
                        if (player.Distance(target.ServerPosition) < QWER.Range - (poutput.CastPosition.Distance(target.ServerPosition)))
                        {
                            QWER.Cast(poutput.CastPosition);
                        }
                    }
                    else
                    {
                        QWER.Cast(poutput.CastPosition);
                    }
                }
            }
            else if (HitChanceNum == 4 && (int)poutput.HitChance > 4)
            {
                List<Vector2> waypoints = target.GetWaypoints();
                float SiteToSite = ((target.MoveSpeed * QWER.CastDelay) + (player.Distance(target.ServerPosition) / QWER.Speed()) - QWER.Width()) * 6;
                float BackToFront = ((target.MoveSpeed * QWER.CastDelay) + (player.Distance(target.ServerPosition) / QWER.Speed()));

                if (player.Distance(waypoints.Last().To3D()) < SiteToSite || player.Distance(target.Position) < SiteToSite)
                    QWER.CastIfHitchanceEquals(target, EloBuddy.SDK.Enumerations.HitChance.High, true);
                else if ((target.ServerPosition.Distance(waypoints.Last().To3D()) > SiteToSite
                    || Math.Abs(player.Distance(waypoints.Last().To3D()) - player.Distance(target.Position)) > BackToFront))
                {
                    if (waypoints.Last().To3D().Distance(player.Position) <= target.Distance(player.Position))
                    {
                        if (player.Distance(target.ServerPosition) < QWER.Range - (poutput.CastPosition.Distance(target.ServerPosition)))
                        {
                            QWER.Cast(poutput.CastPosition);
                        }
                    }
                    else
                    {
                        QWER.Cast(poutput.CastPosition);
                    }
                }
            }
        }


        public override void setUpSpells()
        {
            // Initialize spells
            Q = new Skillshot(SpellSlot.Q, 1075, EloBuddy.SDK.Enumerations.SkillShotType.Linear, 0, 850, 110)
            {
                AllowedCollisionCount = int.MaxValue
            };
            W = new Skillshot(SpellSlot.W, 1000, EloBuddy.SDK.Enumerations.SkillShotType.Circular, 0, int.MaxValue, 1);
            E = new Targeted(SpellSlot.E, 650);
            R = new Skillshot(SpellSlot.R, 625, EloBuddy.SDK.Enumerations.SkillShotType.Circular, 0, int.MaxValue, 400);
        }

        private static bool Combo
        {
            get { return player.Position.CountEnemyHeroesInRangeWithPrediction(1300) != 0; }
        }
        private static bool Farm
        {
            get { return !Combo && player.ManaPercent > 79; }
        }
    }
}