using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using System.Collections.Generic;

namespace ARAMDetFull.Champions
{
    class Leona : Champion
    {
        public Leona()
        {
            ARAMSimulator.champBuild = new Build
            {
                coreItems = new List<ConditionalItem>
                {
                    new ConditionalItem(ItemId.Mercurys_Treads, ItemId.Ninja_Tabi, ItemCondition.ENEMY_AP),
                    new ConditionalItem(ItemId.Sunfire_Cape),
                    new ConditionalItem(ItemId.Banner_of_Command,ItemId.Locket_of_the_Iron_Solari,ItemCondition.ENEMY_AP),
                    new ConditionalItem(ItemId.Banshees_Veil),
                    new ConditionalItem(ItemId.Randuins_Omen),
                    new ConditionalItem(ItemId.Warmogs_Armor),
                },
                startingItems = new List<ItemId>
                {
                    ItemId.Giants_Belt
                }
            };
        }

        public override void useQ(Obj_AI_Base target)
        {
            if (!Q.IsReady())
                return;
            Q.Cast();
        }

        public override void useW(Obj_AI_Base target)
        {
            if (!W.IsReady())
                return;
            W.Cast();
        }

        public override void useE(Obj_AI_Base target)
        {
            if (!E.IsReady() || W.IsReady())
                return;
            var targetHero = target as AIHeroClient;
            if (targetHero == null) return;

            if (//!Sector.inTowerRange(targetHero) &&
                (MapControl.balanceAroundPoint(target.Position.To2D(), 700) >= -1 ||
                 (MapControl.fightIsOn() != null && MapControl.fightIsOn().NetworkId == target.NetworkId)))
                E.Cast(target);
        }

        public override void useR(Obj_AI_Base target)
        {
            if (!R.IsReady())
                return;
            if (target.HealthPercent < 50)
                R.Cast(target);
            else
            {
                (R as Spell.Skillshot).CastIfItWillHit(2, 80);
            }
        }

        public override void setUpSpells()
        {
            Q = new Spell.Active(SpellSlot.Q);
            W = new Spell.Active(SpellSlot.W);
            E = new Spell.Skillshot(SpellSlot.E, 175, SkillShotType.Linear, 250, 2000, 100);
            R = new Spell.Skillshot(SpellSlot.R, 300, SkillShotType.Circular, 1000, 1000, 300)
            {
                AllowedCollisionCount = int.MaxValue
            };
        }

        public override void useSpells()
        {
            var tar = ARAMTargetSelector.getBestTarget(Q.Range);
            if (tar != null) useQ(tar);
            tar = ARAMTargetSelector.getBestTarget(W.Range);
            if (tar != null) useW(tar);
            tar = ARAMTargetSelector.getBestTarget(E.Range);
            if (tar != null) useE(tar);
            tar = ARAMTargetSelector.getBestTarget(R.Range);
            if (tar != null) useR(tar);
        }
    }
}