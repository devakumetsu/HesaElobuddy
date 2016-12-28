using System;
using System.Collections.Generic;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using SharpDX;

namespace ARAMDetFull.Champions
{
    class Lux : Champion
    {
        public Lux()
        {
            ARAMSimulator.champBuild = new Build
            {
                coreItems = new List<ConditionalItem>
                {
                    new ConditionalItem(ItemId.Morellonomicon),
                    new ConditionalItem(ItemId.Sorcerers_Shoes),
                    new ConditionalItem(ItemId.Ludens_Echo),
                    new ConditionalItem(ItemId.Rabadons_Deathcap),
                    new ConditionalItem(ItemId.Void_Staff),
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
            if (Q.IsReady() && target != null)
            {
                Q.Cast(target);
            }
        }

        public override void useW(Obj_AI_Base target)
        {
            if (W.IsReady())
            {
                if (Player.Instance.HealthPercent > 35)
                {
                    if (Player.Instance.CountEnemiesInRange(500)<2)
                    {
                        W.Cast();
                    }
                }
            }
        }

        public override void useE(Obj_AI_Base target)
        {
            if (E.IsReady() && target != null)
            E.Cast(target);
        }


        public override void useR(Obj_AI_Base target)
        {
            if (R.IsReady())
            {
                if (target.HealthPercent >= 30)
                    R.Cast(target);
            }
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

        public override void setUpSpells()
        {
            //Create the spells
            Q = new Spell.Skillshot(SpellSlot.Q, 1175, SkillShotType.Linear, 250, 1200, 70)
            {
                AllowedCollisionCount = 1
            };
            W = new Spell.Skillshot(SpellSlot.W, 1075, SkillShotType.Linear, 350, 1500, 130)
            {
                AllowedCollisionCount = int.MaxValue
            };
            E = new Spell.Skillshot(SpellSlot.E, 1100, SkillShotType.Circular, 250, 1300, 335)
            {
                AllowedCollisionCount = int.MaxValue
            };
            R = new Spell.Skillshot(SpellSlot.R, 3000, SkillShotType.Linear, 1000, int.MaxValue, 110)
            {
                AllowedCollisionCount = int.MaxValue
            };
        }
    }
}
