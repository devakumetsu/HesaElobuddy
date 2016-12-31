using System.Collections.Generic;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using SharpDX;

namespace ARAMDetFull.Champions
{
    class Thresh : Champion
    {
        private const int QFollowTime = 3000;
        private Obj_AI_Base QTarget;
        private double QTick;

        public Thresh()
        {
            ARAMSimulator.champBuild = new Build
            {
                coreItems = new List<ConditionalItem>
                {
                    new ConditionalItem(ItemId.Banner_of_Command,ItemId.Locket_of_the_Iron_Solari,ItemCondition.ENEMY_AP),
                    new ConditionalItem(ItemId.Mercurys_Treads,ItemId.Ninja_Tabi,ItemCondition.ENEMY_AP),
                    new ConditionalItem(ItemId.Sunfire_Cape),
                    new ConditionalItem(ItemId.Banshees_Veil),
                    new ConditionalItem(ItemId.Iceborn_Gauntlet),
                    new ConditionalItem(ItemId.Spirit_Visage,ItemId.Warmogs_Armor,ItemCondition.ENEMY_AP),
                },
                startingItems = new List<ItemId>
                {
                    ItemId.Sheen
                }
            };
        }

        private bool FollowQ
        {
            get { return ARAMDetFull.now <= QTick + QFollowTime; }
        }

        private bool FollowQBlock
        {
            get { return ARAMDetFull.now - QTick >= QFollowTime; }
        }

        public int AllowedCollisionCount { get; private set; }
        public Spell.Active Q2 { get; private set; }

        public override void useQ(Obj_AI_Base target)
        {
            if (!Q.IsReady() || target == null)
                return;
            if (FollowQBlock)
            {
                if (Q.Cast(target))
                {
                    QTick = ARAMDetFull.now;
                    QTarget = target;
                }
            }
            if (FollowQ && safeGap(target))
            {
                //Q.Cast();
            }
            var qpred = Q.GetPrediction(target);
            if (target.IsValidTarget(Q.Range) && Q.IsReady() && qpred.HitChance >= HitChance.High)
            {

                Q.Cast(target);
            }
            var alliesinQrange = ObjectManager.Player.CountAlliesInRange(Q.Range);
            if (Q.Cast(target) && alliesinQrange <= 2)
            {
                Q2.Cast();
            }
        }

        public override void useW(Obj_AI_Base target)
        {
            if (W.IsReady())
                EngageFriendLatern();
        }

        public override void useE(Obj_AI_Base target)
        {
            if (!E.IsReady() || target == null)
                return;
            if (AllyBelowHp(25, E.Range) != null)
            {
                E.Cast(target.Position);
            }
            else
            {
                E.Cast(ReversePosition(ObjectManager.Player.Position, target.Position));
            }
        }
        
        public override void useR(Obj_AI_Base target)
        {
            if (target == null)
                return;
            if (target.IsValidTarget(R.Range) && R.IsReady() && EnemyInRange(2, 500))
            {
                R.Cast();
            }
        }

        public override void useSpells()
        {
            var tar = ARAMTargetSelector.getBestTarget(Q.Range);
            useQ(tar);
            tar = ARAMTargetSelector.getBestTarget(W.Range);
            useW(tar);
            tar = ARAMTargetSelector.getBestTarget(E.Range);
            useE(tar);
            tar = ARAMTargetSelector.getBestTarget(R.Range);
            useR(tar);
        }
        
        public override void setUpSpells()
        {
            //new spells
            Q = new Spell.Skillshot(SpellSlot.Q, 1040, SkillShotType.Linear, (int)0.5f, (int?)1900f, 70);
            {
                AllowedCollisionCount = 0;
            }

            Q2 = new Spell.Active(SpellSlot.Q, 9000);
            W = new Spell.Skillshot(SpellSlot.W, 950, SkillShotType.Circular, 250, int.MaxValue, 10);
            {
                AllowedCollisionCount = int.MaxValue;
            }
            E = new Spell.Skillshot(SpellSlot.E, 480, SkillShotType.Linear, (int)0.25f, int.MaxValue, 50);
            {
                AllowedCollisionCount = int.MaxValue;
            }

            R = new Spell.Active(SpellSlot.R, 350);
        }

        public static bool EnemyInRange(int numOfEnemy, float range)
        {
            return ObjectManager.Player.CountEnemiesInRange((int)range) >= numOfEnemy;
        }

        public static Vector3 ReversePosition(Vector3 positionMe, Vector3 positionEnemy)
        {
            var x = positionMe.X - positionEnemy.X;
            var y = positionMe.Y - positionEnemy.Y;
            return new Vector3(positionMe.X + x, positionMe.Y + y, positionMe.Z);
        }

        public static AIHeroClient AllyBelowHp(int percentHp, float range)
        {
            foreach (var ally in ObjectManager.Get<AIHeroClient>())
            {
                if (ally.IsMe)
                {
                    if (((ObjectManager.Player.Health / ObjectManager.Player.MaxHealth) * 100) < percentHp)
                    {
                        return ally;
                    }
                }
                else if (ally.IsAlly)
                {
                    if (Vector3.Distance(ObjectManager.Player.Position, ally.Position) < range &&
                        ((ally.Health / ally.MaxHealth) * 100) < percentHp)
                    {
                        return ally;
                    }
                }
            }

            return null;
        }

        public void OnPossibleToInterrupt(Obj_AI_Base unit, Interrupter.InterruptableSpellEventArgs spell)
        {
            if (spell.DangerLevel < DangerLevel.High || unit.IsAlly)
            {
                return;
            }
            if (E.IsReady() && E.IsInRange(unit))
                E.Cast(unit.Position);
        }


        private void EngageFriendLatern()
        {
            if (!W.IsReady())
            {
                return;
            }
            
            foreach (var friend in
                EntityManager.Heroes.Allies
                    .Where(
                        hero =>
                            hero.IsAlly && hero.Distance(player) <= W.Range + 200 && hero.Health / hero.MaxHealth * 100 >= 10 &&
                            hero.CountEnemiesInRange(550) >= 1))
            {
                W.Cast(friend.Position);
                return;
            }
        }
    }
}