using System.Collections.Generic;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;

namespace ARAMDetFull.Champions
{
    class Zilean : Champion
    {
        public Zilean()
        {
            ARAMSimulator.champBuild = new Build
            {
                coreItems = new List<ConditionalItem>
                    {
                        new ConditionalItem(ItemId.Athenes_Unholy_Grail),
                        new ConditionalItem(ItemId.Sorcerers_Shoes),
                        new ConditionalItem(ItemId.Rabadons_Deathcap),
                        new ConditionalItem(ItemId.Ludens_Echo),
                        new ConditionalItem(ItemId.Zhonyas_Hourglass),
                        new ConditionalItem(ItemId.Void_Staff),
                    },
                startingItems = new List<ItemId>
                    {
                        ItemId.Needlessly_Large_Rod
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
            if (!Q.IsReady(3500) && player.Mana>150)
                W.Cast();
        }

        public override void useE(Obj_AI_Base target)
        {
            if (!E.IsReady() || target == null)
                return;
                E.CastOnUnit(target);
        }

        public override void useR(Obj_AI_Base target)
        {
            if (target == null)
                return;
        }

        public override void useSpells()
        {
            UltAlly();
            var tar = ARAMTargetSelector.getBestTarget(Q.Range);
            if (tar != null) useQ(tar);
            tar = ARAMTargetSelector.getBestTarget(W.Range);
            useW(tar);
            tar = ARAMTargetSelector.getBestTarget(E.Range);
            if (tar != null) useE(tar);

        }
        
        public override void setUpSpells()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 900, SkillShotType.Circular, (int)0.30f, 2000, 210)
            {
                AllowedCollisionCount = int.MaxValue
            };
            W = new Spell.Active(SpellSlot.W);
            E = new Spell.Targeted(SpellSlot.E, 700);
            R = new Spell.Targeted(SpellSlot.R, 900);
        }
        
        private void UltAlly()
        {
            if (!R.IsReady())
                return;
            var allyMinHP = 30;

            foreach (var hero in EntityManager.Heroes.Allies)
            {
                if (player.HasBuff("Recall") || Shop.CanShop) return;
                if ((hero.HealthPercent <= allyMinHP) && R.IsReady() &&
                    hero.CountEnemiesInRange(700) > 0 &&
                    (hero.Distance(player.ServerPosition) <= R.Range))
                {
                    R.Cast(hero);
                }
            }
        }
    }
}