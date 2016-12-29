using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using System.Collections.Generic;
using SharpDX;

namespace ARAMDetFull.Champions
{
    class Bard : Champion
    {
        public Spell.Skillshot stunQ;

        public Bard()
        {
            ARAMSimulator.champBuild = new Build
            {
                coreItems = new List<ConditionalItem>
                {
                    new ConditionalItem(ItemId.Face_of_the_Mountain),
                    new ConditionalItem(ItemId.Boots_of_Mobility),
                    new ConditionalItem(ItemId.Banshees_Veil),
                    new ConditionalItem(ItemId.Locket_of_the_Iron_Solari),
                    new ConditionalItem(ItemId.Mikaels_Crucible),
                    new ConditionalItem(ItemId.Righteous_Glory),
                },
                startingItems = new List<ItemId>
                {
                    ItemId.Aegis_of_the_Legion
                }
            };
        }

        public override void useQ(Obj_AI_Base target)
        {
            if (!Q.IsReady() || target == null)
                return;
            if (target.IsValidTarget())
            {
                if (!castStunQ(target))
                {
                    Q.Cast(target);
                }
            }
        }

        public override void useW(Obj_AI_Base target)
        {
            if (!W.IsReady())
                return;
            if (player.Mana > 270)
                W.Cast(player.Position.Extend(ARAMSimulator.fromNex.Position, 645).To3D());
        }

        public override void useE(Obj_AI_Base target)
        {
           
        }


        public override void useR(Obj_AI_Base target)
        {
            if (!R.IsReady() || target == null)
                return;
            if (GetEnemys(target) >= 3)
            {
                R.Cast(target);
            }
        }

        public override void useSpells()
        {
            var tar = ARAMTargetSelector.getBestTarget(Q.Range);
            if (tar != null) useQ(tar);
            tar = ARAMTargetSelector.getBestTarget(W.Range);
            useW(tar);
            tar = ARAMTargetSelector.getBestTarget(E.Range);
            if (tar != null) useE(tar);
            tar = ARAMTargetSelector.getBestTarget(R.Range);
            if (tar != null) useR(tar);
        }

        public override void setUpSpells()
        {
            //Create the spells
            Q = new Spell.Skillshot(SpellSlot.Q, 900, SkillShotType.Linear);
            W = new Spell.Active(SpellSlot.W, 900);
            E = new Spell.Active(SpellSlot.E, 1000);
            R = new Spell.Skillshot(SpellSlot.R, 2500, SkillShotType.Circular, 500, 325, 1800);
            stunQ = new Spell.Skillshot(SpellSlot.Q, Q.Range, SkillShotType.Linear);
        }



        private int GetEnemys(Obj_AI_Base target)
        {
            int Enemys = 0;
            foreach (AIHeroClient enemys in ObjectManager.Get<AIHeroClient>())
            {
                var pred = (R as Spell.Skillshot).GetPrediction(enemys);
                if (pred.HitChance >= HitChance.High && !enemys.IsMe && enemys.IsEnemy && Vector3.Distance(player.Position, pred.UnitPosition) <= R.Range)
                {
                    Enemys = Enemys + 1;
                }
            }
            return Enemys;
        }
        private bool castStunQ(Obj_AI_Base target)
        {
            var prediction = stunQ.GetPrediction(target);

            var direction = (player.ServerPosition - prediction.UnitPosition).Normalized();
            var endOfQ = (Q.Range) * direction;

            var checkPoint = prediction.UnitPosition.Extend(player.ServerPosition, -Q.Range / 4);

            if ((prediction.UnitPosition.GetFirstWallPoint(checkPoint.To3D()).HasValue) || (prediction.CollisionObjects.Length == 1))
            {
                Q.Cast(prediction.UnitPosition);
                return true;
            }
            return false;
        }

    }

    public static class VectorHelper
    {
        public static Vector3? GetFirstWallPoint(this Vector3 from, Vector3 to, float step = 25)
        {
            var direction = (to - from).Normalized();

            for (float d = 0; d < from.Distance(to); d = d + step)
            {
                var testPoint = from + d * direction;
                if (NavMesh.GetCollisionFlags(testPoint.X, testPoint.Y).HasFlag(CollisionFlags.Wall) ||
                    NavMesh.GetCollisionFlags(testPoint.X, testPoint.Y).HasFlag(CollisionFlags.Building))
                {
                    return from + d * direction;
                }
            }

            return null;
        }

        public static Vector2? GetFirstWallPoint(this Vector2 from, Vector2 to, float step = 25)
        {
            var direction = (to - from).Normalized();

            for (float d = 0; d < from.Distance(to); d = d + step)
            {
                var testPoint = from + d * direction;
                if (NavMesh.GetCollisionFlags(testPoint.X, testPoint.Y).HasFlag(CollisionFlags.Wall) ||
                    NavMesh.GetCollisionFlags(testPoint.X, testPoint.Y).HasFlag(CollisionFlags.Building))
                {
                    return from + d * direction;
                }
            }

            return null;
        }
    }
}
