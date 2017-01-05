using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using System.Collections.Generic;

namespace ARAMDetFull.Champions
{
    class Braum : Champion
    {
        public Braum()
        {
            ARAMSimulator.champBuild = new Build
            {
                coreItems = new List<ConditionalItem>
                        {
                            new ConditionalItem(ItemId.Boots_of_Mobility),
                            new ConditionalItem(ItemId.Eye_of_the_Equinox),
                            new ConditionalItem(ItemId.Locket_of_the_Iron_Solari),
                            new ConditionalItem(ItemId.Spirit_Visage),
                            new ConditionalItem(ItemId.Zekes_Harbinger),
                            new ConditionalItem(ItemId.Guardian_Angel),
                        },
                startingItems = new List<ItemId>
                {
                    ItemId.Targons_Brace,ItemId.Boots_of_Mobility,ItemId.Health_Potion
                }
            };
        }

        public override void useQ(Obj_AI_Base target)
        {
            if (!Q.IsReady() || target == null)
                return;
            if (Q.IsReady())
                Q.GetPrediction(target);
                Q.Cast(target);
        }

        public override void useW(Obj_AI_Base target)
        {
            if (!W.IsReady())
                return;
            if (W.IsReady())
            {
                //W.Cast();
            }
        }

        public override void useE(Obj_AI_Base target)
        {
            if (!E.IsReady() || target == null)
                return;
            if (E.IsReady())
            E.Cast(target);
        }


        public override void useR(Obj_AI_Base target)
        {
            if (!R.IsReady() || target == null)
                return;
            if (target.IsValidTarget(R.Range) && Player.Instance.CountAllyChampionsInRange(R.Range) <=2)
            {
                R.CastIfWillHit(target,2);
            }
        }

        public override void useSpells()
        {
            var tar = ARAMTargetSelector.getBestTarget(Q.Range);
            if (tar != null) useQ(tar);
            tar = ARAMTargetSelector.getBestTarget(W.Range);
            //useW(tar);
            tar = ARAMTargetSelector.getBestTarget(E.Range);
            if (tar != null) useE(tar);
            tar = ARAMTargetSelector.getBestTarget(R.Range);
            if (tar != null) useR(tar);
        }

        public override void setUpSpells()
        {
            //Create the spells
            Q = new Spell.Skillshot(SpellSlot.Q, 950, SkillShotType.Linear,250,1550,50)
            {
                AllowedCollisionCount = 0
            };
            W = new Spell.Targeted(SpellSlot.W,650);
            E = new Spell.Active(SpellSlot.E);
            R = new Spell.Skillshot(SpellSlot.R, 1200,SkillShotType.Linear,550,1350,108);
        }
    }
}