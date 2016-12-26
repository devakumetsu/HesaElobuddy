using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using System.Collections.Generic;

namespace ARAMDetFull.Champions
{
    class Cassiopeia : Champion
    {
        public Cassiopeia()
        {
            ARAMSimulator.champBuild = new Build
            {
                coreItems = new List<ConditionalItem>
                {
                    new ConditionalItem(ItemId.Rod_of_Ages),
                    new ConditionalItem(ItemId.Sorcerers_Shoes),
                    new ConditionalItem(ItemId.Rabadons_Deathcap),
                    new ConditionalItem(ItemId.Rylais_Crystal_Scepter),
                    new ConditionalItem(ItemId.Liandrys_Torment),
                    new ConditionalItem(ItemId.Zhonyas_Hourglass),
                },
                startingItems = new List<ItemId>
                {
                    ItemId.Catalyst_of_Aeons
                }
            };
        }

        public override void useQ(Obj_AI_Base target)
        {
            if (!Q.IsReady() || target == null)
                return;
            if (target.IsValidTarget(Q.Range+100))
                Q.Cast(target);
        }

        public override void useW(Obj_AI_Base target)
        {
            if (!W.IsReady() || target == null)
                return;
            if (target.IsValidTarget(W.Range + 100))
                W.Cast(target);
        }

        public override void useE(Obj_AI_Base target)
        {
            if (!E.IsReady() || target == null)
                return;
            if ((target.HasBuffOfType(BuffType.Poison)))
            {
                E.Cast(target);
            }
        }

        public override void useR(Obj_AI_Base target)
        {
            if (target == null)
                return;
            if (target.IsValidTarget(R.Range) && R.IsReady())
            {
                R.Cast(target);
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
            Q = new Spell.Skillshot(SpellSlot.Q, 850, SkillShotType.Circular, castDelay: 400, spellWidth: 75);
            W = new Spell.Skillshot(SpellSlot.W, 850, SkillShotType.Circular, spellWidth: 125);
            E = new Spell.Targeted(SpellSlot.E, 700);
            R = new Spell.Skillshot(SpellSlot.R, 825, SkillShotType.Cone, spellWidth: 80);
        }
    }
}
