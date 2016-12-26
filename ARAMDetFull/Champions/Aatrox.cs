using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using System.Collections.Generic;

namespace ARAMDetFull.Champions
{
    class Aatrox : Champion
    {
        public Aatrox()
        {
            ARAMSimulator.champBuild = new Build
            {
                coreItems = new List<ConditionalItem>
                        {
                            new ConditionalItem(ItemId.Mercurys_Treads,ItemId.Ninja_Tabi,ItemCondition.ENEMY_AP),
                            new ConditionalItem(ItemId.Blade_of_the_Ruined_King),
                            new ConditionalItem(ItemId.Ravenous_Hydra_Melee_Only),
                            new ConditionalItem(ItemId.Randuins_Omen),
                            new ConditionalItem(ItemId.The_Black_Cleaver),
                            new ConditionalItem(ItemId.Banshees_Veil),
                        },
                startingItems = new List<ItemId>
                {
                    ItemId.Boots_of_Speed,ItemId.Vampiric_Scepter
                }
            };
        }

        public bool wHealing
        {
            get { return player.HasBuff("AatroxWLife"); }
        }

        public override void useQ(Obj_AI_Base target)
        {
            if (!Q.IsReady() || target == null)
                return;
            if (safeGap(target))
                Q.Cast(target);
        }

        public override void useW(Obj_AI_Base target)
        {
            if (!W.IsReady())
                return;
            if (player.HealthPercent < 40 && !wHealing)
                W.Cast();
            else if (player.HealthPercent < 60 && wHealing)
                W.Cast();
        }

        public override void useE(Obj_AI_Base target)
        {
            if (!E.IsReady() || target == null)
                return;
            E.Cast(target);
        }


        public override void useR(Obj_AI_Base target)
        {
            if (!R.IsReady() || target == null)
                return;
            if (target.IsValidTarget(R.Range))
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
            Q = new Spell.Skillshot(SpellSlot.Q, 650, SkillShotType.Circular, 600, 2000, 250)
            {
                AllowedCollisionCount = int.MaxValue,
                DamageType = DamageType.Physical
            };
            E = new Spell.Skillshot(SpellSlot.E, 1000, SkillShotType.Linear, 250, 1250, 35)
            {
                AllowedCollisionCount = int.MaxValue,
                DamageType = DamageType.Magical
            };
            W = new Spell.Active(SpellSlot.W);
            R = new Spell.Active(SpellSlot.R, 550)
            {
                DamageType = DamageType.Magical
            };
        }
    }
}