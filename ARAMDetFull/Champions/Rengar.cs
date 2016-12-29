using System;
using System.Collections.Generic;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using SharpDX;

namespace ARAMDetFull.Champions
{
    class Rengar : Champion
    {
        public Rengar()
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
                    ItemId.Caulfields_Warhammer
                }
            };
        }

        public override void useQ(Obj_AI_Base target)
        {
            if (Q.IsReady() && Q.IsInRange(target))
            {
                Q.Cast(target);
            }
        }

        public override void useW(Obj_AI_Base target)
        {
            if (W.IsReady())
            {
                if (Player.Instance.HealthPercent > 25)
                {
                    if (Player.Instance.CountEnemiesInRange(W.Range) ==1)
                    {
                        W.Cast();
                    }
                }
            }
        }

        public override void useE(Obj_AI_Base target)
        {
            if (E.IsReady() && E.IsInRange(target))
                E.Cast(target);
        }


        public override void useR(Obj_AI_Base target)
        {
            if (R.IsReady())
            {
                if (player.CountEnemiesInRange(500) >= 2 && player.CountAlliesInRange(500) <= 2)
                { 
                    R.Cast(target);
                }
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
            Q = new Spell.Skillshot(SpellSlot.Q, 326, EloBuddy.SDK.Enumerations.SkillShotType.Cone, 250, 3000, 150, DamageType.Physical)
            {
                ConeAngleDegrees = 180,
                AllowedCollisionCount = int.MaxValue,
                MinimumHitChance = EloBuddy.SDK.Enumerations.HitChance.High,

            };
            /*Q2 = new Spell.Skillshot(SpellSlot.Q, 450, EloBuddy.SDK.Enumerations.SkillShotType.Linear, 500, 3000, 150, DamageType.Physical)
            {
                AllowedCollisionCount = int.MaxValue,
            };*/
            W = new Spell.Active(SpellSlot.W, 450, DamageType.Magical);
            E = new Spell.Skillshot(SpellSlot.E, 1000, EloBuddy.SDK.Enumerations.SkillShotType.Linear, 250, 1500, 70, DamageType.Physical)
            {
                AllowedCollisionCount = 1,
                MinimumHitChance = EloBuddy.SDK.Enumerations.HitChance.High,
            };
            R = new Spell.Active(SpellSlot.R, 2000);
        }
    }
}
