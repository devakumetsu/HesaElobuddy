using System;
using System.Collections.Generic;
using System.Linq;
using SharpDX;
using static EloBuddy.SDK.Spell;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;

namespace ARAMDetFull.Champions
{
    class Ziggs : Champion
    {
        public static SpellBase Q1;
        public static SpellBase Q2;
        public static SpellBase Q3;

        private int UseSecondWT = 0;

        public Ziggs()
        {
            ARAMSimulator.champBuild = new Build
            {
                coreItems = new List<ConditionalItem>
                {
                    new ConditionalItem(ItemId.Archangels_Staff),
                    new ConditionalItem(ItemId.Sorcerers_Shoes),
                    new ConditionalItem(ItemId.Rabadons_Deathcap),
                    new ConditionalItem(ItemId.Ludens_Echo),
                    new ConditionalItem(ItemId.Void_Staff),
                    new ConditionalItem(ItemId.Banshees_Veil),
                },
                startingItems = new List<ItemId>
                {
                    ItemId.Tear_of_the_Goddess,ItemId.Boots_of_Speed
                }
            };
        }

        public override void useQ(Obj_AI_Base target)
        {
            if (!Q1.IsReady() || target == null)
                return;
            CastQ(target);
        }

        public override void useW(Obj_AI_Base target)
        {
            if (!W.IsReady())
                return;
            var po = E.GetPrediction(target);
            var dist = po.UnitPosition.Distance(player.Position);
            if (dist < 900)
            {
                var pos = player.Position.Extend(po.UnitPosition, dist + 90);
                W.Cast(pos.To3D());
            }

        }

        public override void useE(Obj_AI_Base target)
        {
            if (!E.IsReady() || target == null)
                return;

            //var distToTar = target.Distance(player);

            E.Cast(target);
        }


        public override void useR(Obj_AI_Base target)
        {
            if (!R.IsReady() || target == null)
                return;
            if ((ObjectManager.Player.GetSpellDamage(target, SpellSlot.Q) +
                         ObjectManager.Player.GetSpellDamage(target, SpellSlot.W) +
                         ObjectManager.Player.GetSpellDamage(target, SpellSlot.E) +
                         ObjectManager.Player.GetSpellDamage(target, SpellSlot.R) > target.Health) &&
                        ObjectManager.Player.Distance(target) <= Q2.Range)
            {
                //R.Delay = 2000 + 1500 * target.Distance(ObjectManager.Player) / 5300;
                R.Cast(target);
            }
        }

        public override void useSpells()
        {
            var tar = ARAMTargetSelector.getBestTarget(W.Range);
            if (tar != null) useW(tar);
            tar = ARAMTargetSelector.getBestTarget(Q2.Range - 100);
            if (tar != null) useQ(tar);
            
            tar = ARAMTargetSelector.getBestTarget(E.Range);
            if (tar != null) useE(tar);
            var target = ARAMTargetSelector.getBestTarget(R.Range);
            if (target != null) useR(target);
            
            if (Core.GameTickCount - UseSecondWT < 500 && ObjectManager.Player.Spellbook.GetSpell(SpellSlot.W).Name == "ziggswtoggle")
            {
                W.Cast(ObjectManager.Player.ServerPosition);
            }

            //R aoe in teamfights
            if (R.IsReady() && target!= null)
            {
                var alliesarround = 0;
                foreach (var ally in EntityManager.Heroes.Allies)
                {
                    if (ally.IsAlly && !ally.IsMe && ally.IsValidTarget(float.MaxValue, false) && ally.Distance(target) < 700)
                    {
                        alliesarround++;
                    }
                }

                switch (alliesarround)
                {
                    case 2:
                        R.CastIfWillHit(target, 2);
                    break;
                    case 3:
                        R.CastIfWillHit(target, 3);
                    break;
                    case 4:
                        R.CastIfWillHit(target, 4);
                    break;
                }
            }

            foreach (var pos in from enemy in EntityManager.Heroes.Enemies
                where
                    enemy.IsValidTarget() &&
                    enemy.Distance(ObjectManager.Player) <=
                    enemy.BoundingRadius + enemy.AttackRange + ObjectManager.Player.BoundingRadius &&
                    enemy.IsMelee
                let direction =
                    (enemy.ServerPosition.To2D() - ObjectManager.Player.ServerPosition.To2D()).Normalized()
                let pos = ObjectManager.Player.ServerPosition.To2D()
                select pos + Math.Min(200, Math.Max(50, enemy.Distance(ObjectManager.Player) / 2)) * direction)
            {
                W.Cast(pos.To3D());
                UseSecondWT = Core.GameTickCount;
            }
            
            
        }

        public override void setUpSpells()
        {
            Q1 = Q = new Spell.Skillshot(SpellSlot.Q, 850, SkillShotType.Circular, 300, 1700, 130);
            Q2 = new Spell.Skillshot(SpellSlot.Q, 1125, SkillShotType.Circular, 250 + Q1.CastDelay, 1700, 130);
            Q3 = new Spell.Skillshot(SpellSlot.Q, 1400, SkillShotType.Circular, 300 + Q2.CastDelay, 1700, 140);

            W = new Spell.Skillshot(SpellSlot.W, 1000, SkillShotType.Circular, 250, 1750, 275);
            E = new Spell.Skillshot(SpellSlot.E, 900, SkillShotType.Circular, 500, 1750, 100);
            R = new Spell.Skillshot(SpellSlot.R, 5300, SkillShotType.Circular, 2000, 1500, 500);
        }

        private void CastQ(Obj_AI_Base target)
        {
            PredictionResult prediction;

            if (ObjectManager.Player.Distance(target) < Q1.Range)
            {
                var oldrange = Q1.Range;
                Q1.Range = Q2.Range;
                prediction = Q1.GetPrediction(target, true);
                Q1.Range = oldrange;
            }
            else if (ObjectManager.Player.Distance(target) < Q2.Range)
            {
                var oldrange = Q2.Range;
                Q2.Range = Q3.Range;
                prediction = Q2.GetPrediction(target, true);
                Q2.Range = oldrange;
            }
            else if (ObjectManager.Player.Distance(target) < Q3.Range)
            {
                prediction = Q3.GetPrediction(target, true);
            }
            else
            {
                return;
            }

            if (prediction.HitChance >= HitChance.High)
            {
                if (ObjectManager.Player.ServerPosition.Distance(prediction.CastPosition) <= Q1.Range + Q1.Width())
                {
                    Vector3 p;
                    if (ObjectManager.Player.ServerPosition.Distance(prediction.CastPosition) > 300)
                    {
                        p = prediction.CastPosition -
                            100 *
                            (prediction.CastPosition.To2D() - ObjectManager.Player.ServerPosition.To2D()).Normalized()
                                .To3D();
                    }
                    else
                    {
                        p = prediction.CastPosition;
                    }

                    Q1.Cast(p);
                }
                else if (ObjectManager.Player.ServerPosition.Distance(prediction.CastPosition) <= ((Q1.Range + Q2.Range) / 2))
                {
                    var p = ObjectManager.Player.ServerPosition.To2D().Extend(prediction.CastPosition.To2D(), Q1.Range - 100);

                    if (!CheckQCollision(target, prediction.UnitPosition, p.To3D()))
                    {
                        Q1.Cast(p.To3D());
                    }
                }
                else
                {
                    var p = ObjectManager.Player.ServerPosition.To2D() +
                            Q1.Range *
                            (prediction.CastPosition.To2D() - ObjectManager.Player.ServerPosition.To2D()).Normalized
                                ();

                    if (!CheckQCollision(target, prediction.UnitPosition, p.To3D()))
                    {
                        Q1.Cast(p.To3D());
                    }
                }
            }
        }

        public override void farm()
        {
            var laneClear = true;
            if (player.ManaPercent<65)
            {
                return;
            }

            var rangedMinions = EntityManager.MinionsAndMonsters.EnemyMinions.Where(x => !x.IsDead && x.IsMelee && x.IsInRange(ObjectManager.Player.ServerPosition, Q2.Range));
            var allMinions = EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy, ObjectManager.Player.ServerPosition, Q2.Range);

            var useQi = 2;
            var useWi = 2;
            var useEi = 2;
            var useQ = (laneClear && (useQi == 1 || useQi == 2)) || (!laneClear && (useQi == 0 || useQi == 2));
            var useW = (laneClear && (useWi == 1 || useWi == 2)) || (!laneClear && (useWi == 0 || useWi == 2));
            var useE = (laneClear && (useEi == 1 || useEi == 2)) || (!laneClear && (useEi == 0 || useEi == 2));

            if (laneClear)
            {
                if (Q1.IsReady() && useQ)
                {
                    var rangedLocation = Q2.GetCircularFarmLocation(rangedMinions, Q2.Width());
                    var location = Q2.GetCircularFarmLocation(allMinions, Q2.Width());

                    var bLocation = (location.HitNumber > rangedLocation.HitNumber + 1) ? location : rangedLocation;

                    if (bLocation.HitNumber > 0)
                    {
                        Q2.Cast(bLocation.CastPosition);
                    }
                }

                if (W.IsReady() && useW)
                {
                    var dmgpct = new[] { 25, 27.5, 30, 32.5, 35 }[W.Level - 1];
                    var killableTurret = ObjectManager.Get<Obj_AI_Turret>().FirstOrDefault(x => x.IsEnemy && ObjectManager.Player.Distance(x.Position) <= W.Range && x.HealthPercent < dmgpct);
                    if (killableTurret != null)
                    {
                        W.Cast(killableTurret.Position);
                    }
                }

                if (E.IsReady() && useE)
                {
                    var rangedLocation = E.GetCircularFarmLocation(rangedMinions, E.Width() * 2);
                    var location = E.GetCircularFarmLocation(allMinions, E.Width() * 2);

                    var bLocation = (location.HitNumber > rangedLocation.HitNumber + 1) ? location : rangedLocation;

                    if (bLocation.HitNumber > 2)
                    {
                        E.Cast(bLocation.CastPosition);
                    }
                }
            }
            else
            {
                if (useQ && Q1.IsReady())
                {
                    foreach (var minion in allMinions)
                    {
                        if (!Player.Instance.IsInAutoAttackRange(minion))
                        {
                            var Qdamage = ObjectManager.Player.GetSpellDamage(minion, SpellSlot.Q) * 0.75;

                            if (Qdamage > Q1.GetHealthPrediction(minion))
                            {
                                Q2.Cast(minion);
                            }
                        }
                    }
                }

                if (E.IsReady() && useE)
                {
                    var rangedLocation = E.GetCircularFarmLocation(rangedMinions, E.Width() * 2);
                    var location = E.GetCircularFarmLocation(allMinions, E.Width() * 2);

                    var bLocation = (location.HitNumber > rangedLocation.HitNumber + 1) ? location : rangedLocation;

                    if (bLocation.HitNumber > 2)
                    {
                        E.Cast(bLocation.CastPosition);
                    }
                }
            }
        }

        private bool CheckQCollision(Obj_AI_Base target, Vector3 targetPosition, Vector3 castPosition)
        {
            var direction = (castPosition.To2D() - ObjectManager.Player.ServerPosition.To2D()).Normalized();
            var firstBouncePosition = castPosition.To2D();
            var secondBouncePosition = firstBouncePosition + direction * 0.4f * ObjectManager.Player.ServerPosition.To2D().Distance(firstBouncePosition);
            var thirdBouncePosition = secondBouncePosition + direction * 0.6f * firstBouncePosition.Distance(secondBouncePosition);
            //TODO: Check for wall collision.
            if (thirdBouncePosition.Distance(targetPosition.To2D()) < Q1.Width() + target.BoundingRadius)
            {
                //Check the second one.
                foreach (var minion in ObjectManager.Get<Obj_AI_Minion>())
                {
                    if (minion.IsValidTarget(3000))
                    {
                        var predictedPos = Q2.GetPrediction(minion);
                        if (predictedPos.UnitPosition.To2D().Distance(secondBouncePosition) < Q2.Width() + minion.BoundingRadius)
                        {
                            return true;
                        }
                    }
                }
            }
            if (secondBouncePosition.Distance(targetPosition.To2D()) < Q1.Width() + target.BoundingRadius || thirdBouncePosition.Distance(targetPosition.To2D()) < Q1.Width() + target.BoundingRadius)
            {
                //Check the first one
                foreach (var minion in ObjectManager.Get<Obj_AI_Minion>())
                {
                    if (minion.IsValidTarget(3000))
                    {
                        var predictedPos = Q1.GetPrediction(minion);
                        if (predictedPos.UnitPosition.To2D().Distance(firstBouncePosition) <
                            Q1.Width() + minion.BoundingRadius)
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
            return true;
        }
    }
}
