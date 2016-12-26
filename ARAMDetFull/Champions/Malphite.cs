using System.Collections.Generic;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;

namespace ARAMDetFull.Champions
{
    class Malphite : Champion
    {

        public Malphite()
        {
            ARAMSimulator.champBuild = new Build
            {
                coreItems = new List<ConditionalItem>
                {
                    new ConditionalItem(ItemId.Sunfire_Cape),
                    new ConditionalItem(ItemId.Mercurys_Treads,ItemId.Ninja_Tabi,ItemCondition.ENEMY_AP),
                    new ConditionalItem(ItemId.Frozen_Heart),
                    new ConditionalItem(ItemId.Iceborn_Gauntlet),
                    new ConditionalItem(ItemId.Spirit_Visage),
                    new ConditionalItem(ItemId.Banshees_Veil),
                },
                startingItems = new List<ItemId>
                {
                    ItemId.Giants_Belt
                }
            };
        }

        public override void useQ(Obj_AI_Base target)
        {
            if (!Q.IsReady() || target == null)
                return;
            Q.CastOnUnit(target);
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
            E.Cast(target);
        }

        public override void useR(Obj_AI_Base target)
        {
            if (target == null)
                return;
            if (target.IsValidTarget(R.Range + 100) && R.IsReady())
            {
                if (target.HealthPercent < 50 && safeGap(target))
                    R.Cast(target);
                else
                    R.CastIfWillHit(target, 2);
            }
        }

        public override void setUpSpells()
        {

            Q = new Spell.Targeted(SpellSlot.Q, 625);
            W = new Spell.Active(SpellSlot.W, 200);
            E = new Spell.Active(SpellSlot.E, 200);
            R = new Spell.Skillshot(SpellSlot.R, 1000, SkillShotType.Circular, 250, 1500, 270);
            /* OLD SPELLS
            Q = new Spell.Targeted(SpellSlot.Q, 625);
            W = new Spell.Active(SpellSlot.W, 200);
            E = new Spell.Active(SpellSlot.E, 200);
            R = new Spell.Skillshot(SpellSlot.E, 1000,SkillShotType.Circular,250,1500,270);*/
            //R.SetSkillshot(0.25f, 270f, 1500f, true, SkillshotType.SkillshotCircle);
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
    }
}