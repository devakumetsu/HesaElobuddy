using System;
using System.Collections.Generic;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using SharpDX;

namespace ARAMDetFull.Champions
{
    class ChoGath : Champion
    {
        public ChoGath()
        {
            ARAMSimulator.champBuild = new Build
            {
                coreItems = new List<ConditionalItem>
                {
                    new ConditionalItem(ItemId.Hextech_Protobelt_01),
                    new ConditionalItem(ItemId.Boots_of_Swiftness),
                    new ConditionalItem(ItemId.Ludens_Echo),
                    new ConditionalItem(ItemId.Rabadons_Deathcap),
                    new ConditionalItem(ItemId.Void_Staff),
                    new ConditionalItem(ItemId.Warmogs_Armor),
                },
                startingItems = new List<ItemId>
                {
                    ItemId.Chalice_of_Harmony, ItemId.Boots_of_Speed
                }
            };
        }

        public override void useQ(Obj_AI_Base target)
        {
            if (Q.IsReady())
            {
                Q.Cast(target);
            }
        }

        public override void useW(Obj_AI_Base target)
        {
            if (W.IsReady())
            {
                //if (Player.Instance.CountEnemiesInRange(W.Range) <= 1)
                {
                    W.Cast(target);
                }
            }
        }

        public override void useE(Obj_AI_Base target)
        {
            if (E.IsReady()) return;
             //E.Cast(target);
        }


        public override void useR(Obj_AI_Base target)
        {
            if (R.IsReady())
            {
                if (target.HealthPercent >30)
                    R.Cast(target);
            }
        }

        public override void useSpells()
        {
            var tar = ARAMTargetSelector.getBestTarget(Q.Range);
            if (tar != null) useQ(tar);
            tar = ARAMTargetSelector.getBestTarget(W.Range);
            if (tar != null) useW(tar);
            //if (tar != null) useE(tar);
            tar = ARAMTargetSelector.getBestTarget(R.Range);
            if (tar != null) useR(tar);

        }

        public override void setUpSpells()
        {
            //Create the spells
            Q = new Spell.Skillshot(SpellSlot.Q, 950, SkillShotType.Circular, 750, int.MaxValue, 175);
            W = new Spell.Skillshot(SpellSlot.W, 650, SkillShotType.Cone, 250, 1750, 100);
            E = new Spell.Active(SpellSlot.E);
            R = new Spell.Targeted(SpellSlot.R, 500);
        }
    }
}