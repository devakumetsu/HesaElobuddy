using System;
using System.Collections.Generic;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using SharpDX;

namespace ARAMDetFull.Champions
{
    class Veigar : Champion
    {
        public Veigar()
        {
            ARAMSimulator.champBuild = new Build
            {
                coreItems = new List<ConditionalItem>
                {
                    new ConditionalItem(ItemId.Rabadons_Deathcap),
                    new ConditionalItem(ItemId.Sorcerers_Shoes),
                    new ConditionalItem(ItemId.Ludens_Echo),
                    new ConditionalItem(ItemId.Void_Staff),
                    new ConditionalItem(ItemId.Zhonyas_Hourglass),
                    new ConditionalItem(ItemId.Banshees_Veil,ItemId.Zhonyas_Hourglass,ItemCondition.ENEMY_AP),
                },
                startingItems = new List<ItemId>
                {
                    ItemId.Blasting_Wand,
                    ItemId.Boots_of_Speed
                }
            };
        }

        public override void useQ(Obj_AI_Base target)
        {
            if (!Q.IsReady() || target == null)
                return;
            Q.Cast(target);
        }

        public override void useW(Obj_AI_Base target)
        {
            if (!W.IsReady() || target == null)
                return;
            W.Cast(target);
        }

        public override void useE(Obj_AI_Base target)
        {
            if (!E.IsReady() || target == null)
                return;
            Vector3? cagePos = GetCageCastPosition(target);
            if (cagePos != null)
                E.Cast((Vector3)cagePos);
        }
        
        public override void useR(Obj_AI_Base target)
        {
            if (!R.IsReady() || target == null)
                return;
            if (target.Health > 50 && target.Health < R.GetDamage(target) + 100)
                R.CastOnUnit(target);
        }

        public override void useSpells()
        {
            if (W.IsReady())
            {
                var target =
                    ObjectManager.Get<AIHeroClient>()
                        .FirstOrDefault(h => h.IsValidTarget(W.Range) /*&& h.IsStunned() >= W.CastDelay - 0.5f*/);
                if (target != null && target.IsStunned)
                    W.Cast(target);
            }

            var tar = ARAMTargetSelector.getBestTarget(Q.Range);
            if (tar != null) useQ(tar);
            else LastHitQ(true);
            tar = ARAMTargetSelector.getBestTarget(W.Range);
            if (tar != null) useW(tar);
            tar = ARAMTargetSelector.getBestTarget(E.Range + 300);
            if (tar != null) useE(tar);
            tar = ARAMTargetSelector.getBestTarget(R.Range);
            if (tar != null) useR(tar);
        }

        private static float _widthSqr;
        private static float _radius;
        private static float _radiusSqr;

        public override void setUpSpells()
        {
            //Create the spells
            Q = new Spell.Skillshot(SpellSlot.Q, 950, SkillShotType.Linear, 250, 2000, 70);
            W = new Spell.Skillshot(SpellSlot.W, 900, SkillShotType.Circular, 1250, 0, 125);
            E = new Spell.Targeted(SpellSlot.E, 650);
            R = new Spell.Targeted(SpellSlot.R, 650);
            var EWidth = 700;
            _widthSqr = EWidth * EWidth;
            _radius = 450;
            _radiusSqr = _radius * _radius;
        }
        
        public Vector3? GetCageCastPosition(Obj_AI_Base target)
        {
            // Get target position after 0.2 seconds
            var prediction = E.GetPrediction(target);

            // Validate single cast position
            if (prediction.HitChance < HitChance.High)
                return null;

            // Check if there are other targets around that could be stunned
            var nearTargets = ObjectManager.Get<AIHeroClient>().Where(
                h =>
                    h.NetworkId != target.NetworkId &&
                    h.IsValidTarget(E.Range + _radius) &&
                    h.Distance(target, true) < _widthSqr);

            foreach (var target2 in nearTargets)
            {
                // Get target2 position after 0.2 seconds
                var prediction2 = E.GetPrediction(target2);

                // Validate second cast position
                if (prediction2.HitChance < HitChance.High ||
                    prediction.UnitPosition.Distance(prediction2.UnitPosition, true) > _widthSqr)
                    continue;

                // Calculate middle point and perpendicular
                var distanceSqr = prediction.UnitPosition.Distance(prediction2.UnitPosition, true);
                var distance = Math.Sqrt(distanceSqr);
                var middlePoint = (prediction.UnitPosition + prediction2.UnitPosition) / 2;
                var perpendicular = (prediction.UnitPosition - prediction2.UnitPosition).Normalized().To2D().Perpendicular();

                // Calculate cast poistion
                var length = (float)Math.Sqrt(_radiusSqr - distanceSqr);
                var castPosition = middlePoint.To2D() + perpendicular * length;

                // Validate cast position
                if (castPosition.Distance(player.Position.To2D(), true) > _radiusSqr)
                    castPosition = middlePoint.To2D() - perpendicular * length;
                // Validate again, if failed continue for loop
                if (castPosition.Distance(player.Position.To2D(), true) > _radiusSqr)
                    continue;

                // Found valid second cast position
                return castPosition.To3D();
            }

            // Returning single cast position
            return prediction.UnitPosition.Extend(player.Position, _radius).To3D();
        }

        public void farmQ()
        {
            if (player.ManaPercent < 35 || !Q.IsReady())
                return;

            var mins = EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy, ObjectManager.Player.Position, Q.Range).Where(min => min.Health < Q.GetDamage(min)).ToList();

            var minsQ = Q.GetCircularFarmLocation(mins, Q.Width());
            if(minsQ.HitNumber > 0)
            {
                Q.Cast(minsQ.CastPosition);
            }
            //var positions = MinionManager.GetMinionsPredictedPositions(mins, Q.Delay, Q.Width, Q.Speed, Q.From, Q.Range, true, SkillshotType.SkillshotLine);
        }

        private void LastHitQ(bool auto = false)
        {
            if (!Q.IsReady())
            {
                return;
            }
            var minions = EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy, ObjectManager.Player.ServerPosition, Q.Range).Where(m => m.Distance(player) < Q.Range);
            var objAiBases = minions as Obj_AI_Base[] ?? minions.ToArray();
            if (objAiBases.Any())
            {
                Obj_AI_Base target = null;
                foreach (var minion in objAiBases)
                {
                    float minPHP = Prediction.Health.GetPrediction(minion, (int)(player.Distance(minion) / Q.Speed()));
                    if (minPHP <= 0 || minPHP > Q.GetDamage(minion))
                        continue;

                    Q.CastIfHitchanceEquals(minion, HitChance.High, true);
                    /*
                    var collision = Q.GetCollision(player.Position.To2D(), new List<Vector2>() { player.Position.Extend(minion.Position, Q.Range).To2D() }, 70f);
                    if (collision.Count <= 2 || collision[0].NetworkId == minion.NetworkId || collision[1].NetworkId == minion.NetworkId)
                    {
                        if (collision.Count == 1)
                        {
                            Q.Cast(minion);
                        }
                        else
                        {
                            var other = collision.FirstOrDefault(c => c.NetworkId != minion.NetworkId);
                            if (other != null && (player.GetAutoAttackDamage(other) * 2 > other.Health - Q.GetDamage(other)) && Prediction.Health.GetPrediction(minion, 1500) > 0 && Q.GetDamage(other) < other.Health)
                            {
                                if (Orbwalker.CanAutoAttack)
                                {
                                    Player.IssueOrder(GameObjectOrder.AutoAttack, other);
                                }
                            }
                            else
                            {
                                Q.Cast(minion);
                            }
                        }
                    }*/
                }
            }
        }
    }
}