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
    class Yasuo : Champion
    {
        //private SpellBase Q2;
        private SpellBase Q3;
        private SpellBase E2;

        public Yasuo()
        {
            ARAMSimulator.champBuild = new Build
            {
                coreItems = new List<ConditionalItem>
                {
                    new ConditionalItem(ItemId.Statikk_Shiv),
                    new ConditionalItem(ItemId.Mercurys_Treads),
                    new ConditionalItem(ItemId.Infinity_Edge),
                    new ConditionalItem(ItemId.Phantom_Dancer),
                    new ConditionalItem(ItemId.Warmogs_Armor),
                    new ConditionalItem(ItemId.Ravenous_Hydra_Melee_Only,ItemId.Banshees_Veil,ItemCondition.ENEMY_LOSING),
                },
                startingItems = new List<ItemId>
                {
                    ItemId.Zeal
                }
            };
        }
        
        public override void useQ(Obj_AI_Base target)
        {
            if (Q.IsReady())
                if (HaveQ3)
                    Q3.Cast(target);
                else
                {
                    Q.Cast(target);
                }
        }

        public override void useW(Obj_AI_Base target)
        {
            if (W.IsReady()) ;
            W.Cast(target.Position);
        }

        public override void useE(Obj_AI_Base target)
        {
            if (!E.IsReady() || target == null)
                return;
            if (safeGap(target))
            {
                if(!gapCloseE(target.Position ,new List<AIHeroClient>() {target as AIHeroClient}))
                    useESmart(target as AIHeroClient);
            }
        }
        
        public override void useR(Obj_AI_Base target)
        {
            if (!R.IsReady() || target == null)
                return;
            //if (target.IsValid)
            //R.Cast();
        }

        public override void useSpells()
        {
            var tar = ARAMTargetSelector.getBestTarget(HaveQ3 ? Q3.Range : Q.Range);
            if (tar != null) useQ(tar);
            tar = ARAMTargetSelector.getBestTarget(W.Range);
            if (tar != null) useW(tar);
            tar = ARAMTargetSelector.getBestTarget(E.Range+600);
            if (tar != null) useE(tar);
            tar = ARAMTargetSelector.getBestTarget(R.Range);
            //if (tar != null) useR(tar);

        }

        public override void farm()
        {
            if (Q.IsReady())
            {
                var minions = EntityManager.MinionsAndMonsters.EnemyMinions.Where(x => !x.IsDead && x.IsInRange(player.ServerPosition, Q.Range));
                var farmL = Q.GetLineFarmLocation(minions, (int) Q.Range);
                if (farmL.HitNumber > 0) Q.Cast(farmL.CastPosition);
            }
        }

        public bool useESmart(AIHeroClient target, List<AIHeroClient> ignore = null)
        {
            if (!E.IsReady())
                return false;
            float trueAARange = player.AttackRange + target.BoundingRadius;
            float trueERange = target.BoundingRadius + E.Range;

            float dist = player.Distance(target);
            Vector2 dashPos = new Vector2();
            if (target.IsMoving && target.Path.Count() != 0)
            {
                Vector2 tpos = target.Position.To2D();
                Vector2 path = target.Path[0].To2D() - tpos;
                path.Normalize();
                dashPos = tpos + (path * 100);
            }
            float targ_ms = (target.IsMoving && player.Distance(dashPos) > dist) ? target.MoveSpeed : 0;
            float msDif = (player.MoveSpeed - targ_ms) == 0 ? 0.0001f : (player.MoveSpeed - targ_ms);
            float timeToReach = (dist - trueAARange) / msDif;
            if (dist > trueAARange && dist < E.Range)
            {
                if (timeToReach > 1.7f || timeToReach < 0.0f)
                {
                    E.Cast(target);
                }
            }
            return false;
        }

        public bool gapCloseE(Vector3 pos, List<AIHeroClient> ignore = null)
        {
            if (!E.IsReady()) return false;

            Vector3 pPos = player.ServerPosition;
            Obj_AI_Base bestEnem = null;
            
            float distToPos = player.Distance(pos);
            Vector3 bestLoc = pPos + (Vector3.Normalize(pos - pPos) * (player.MoveSpeed * 0.35f));
            float bestDist = pos.Distance(pPos) - 50;
            try
            {
                foreach (Obj_AI_Base enemy in ObjectManager.Get<Obj_AI_Base>().Where(ob => enemyIsJumpable(ob, ignore)))
                {
                    float trueRange = E.Range + enemy.BoundingRadius;
                    float distToEnem = player.Distance(enemy);
                    if (distToEnem < trueRange && distToEnem > 15)
                    {
                        Vector3 posAfterE = pPos + (Vector3.Normalize(enemy.Position - pPos) * E.Range);
                        float distE = pos.Distance(posAfterE);
                        if (distE < bestDist)
                        {
                            bestLoc = posAfterE;
                            bestDist = distE;
                            bestEnem = enemy;
                            // Console.WriteLine("Gap to best enem");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            if (bestEnem != null)
            {
                Console.WriteLine("should use gap");
                E.Cast(bestEnem);
                return true;
            }
            return false;

        }

        public bool enemyIsJumpable(Obj_AI_Base enemy, List<AIHeroClient> ignore = null)
        {
            if (enemy.IsValid && enemy.IsEnemy && !enemy.IsInvulnerable && !enemy.MagicImmune && !enemy.IsDead && !(enemy is FollowerObject))
            {
                if (ignore != null)
                {
                    foreach (AIHeroClient ign in ignore)
                    {
                        if (ign.NetworkId == enemy.NetworkId) return false;
                    }
                }
                foreach (BuffInstance buff in enemy.Buffs)
                {
                    if (buff.Name == "YasuoDashWrapper") return false;
                }
                return true;
            }
            return false;
        }

        public override void setUpSpells()
        {
            //Create the spells
            Q = new Spell.Skillshot(SpellSlot.Q, 505, SkillShotType.Linear, (int)GetQDelay, int.MaxValue, 20)
            {
                AllowedCollisionCount = int.MaxValue
            };
            Q3 = new Spell.Skillshot(SpellSlot.Q, 250, SkillShotType.Circular, (int)0.001f, int.MaxValue, 220)
            {
                AllowedCollisionCount = int.MaxValue
            };
            W = new Spell.Skillshot(SpellSlot.W, 400, SkillShotType.Cone, (int)0.25f, int.MaxValue)
            {
                AllowedCollisionCount = int.MaxValue
            };
            E = new Spell.Targeted(SpellSlot.E, 475);
            E2 = new Spell.Skillshot(E.Slot, E.Range, SkillShotType.Linear, Q3.CastDelay, 1200)
            {
                AllowedCollisionCount = int.MaxValue
            };
            R = new Spell.Targeted(SpellSlot.R, 1200);
        }

        private bool HaveQ3
        {
            get { return player.HasBuff("YasuoQ3W"); }
        }

        private float GetQDelay
        {
            get { return 0.4f * (1 - Math.Min((player.AttackSpeedMod - 1) * 0.58f, 0.66f)); }
        }

        private float GetQ2Delay
        {
            get { return 0.5f * (1 - Math.Min((player.AttackSpeedMod - 1) * 0.58f, 0.66f)); }
        }

        private float GetQ3Delay
        {
            get { return 0.6f * (1 - Math.Min((player.AttackSpeedMod - 1) * 0.58f, 0.66f)); }
        }

    }
}
