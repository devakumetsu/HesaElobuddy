using EloBuddy;
using EloBuddy.SDK;
using System.Collections.Generic;

namespace ARAMDetFull.Champions
{
    class AurelionSol : Champion
    {

        public AurelionSol()
        {
            ARAMSimulator.champBuild = new Build
            {
                coreItems = new List<ConditionalItem>
                {
                    new ConditionalItem(ItemId.Rylais_Crystal_Scepter),
                    new ConditionalItem(ItemId.Mercurys_Treads, ItemId.Ninja_Tabi, ItemCondition.ENEMY_AP),
                    new ConditionalItem(ItemId.Rod_of_Ages),
                    new ConditionalItem(ItemId.Abyssal_Scepter, ItemId.Zhonyas_Hourglass, ItemCondition.ENEMY_AP),
                    new ConditionalItem(ItemId.Rabadons_Deathcap),
                    new ConditionalItem(ItemId.Ludens_Echo),
                },
                startingItems = new List<ItemId>
                {
                    ItemId.Giants_Belt
                }
            };
        }

        public override void useQ(Obj_AI_Base target)
        {
            if (!W.IsReady())
                Q.Cast(target);
        }

        public override void useW(Obj_AI_Base target)
        {
            if (!W.IsReady())
                return;
            if (!HasPassive())
            {
                if (target.IsValidTarget(350))
                {
                    return;
                }

                if (player.Distance(target) > 350 && player.Distance(target) < W.Range)
                {
                    W.Cast();
                }
            }
            else if (HasPassive())
            {
                if (player.Distance(target) > 350 && player.Distance(target) < W.Range + 100)
                {
                    return;
                }

                if (player.Distance(target) > 350 + 150)
                {
                    W.Cast();
                }
            }
        }

        public override void useE(Obj_AI_Base target)
        {
        }

        public override void useR(Obj_AI_Base target)
        {
            if (R.IsReady())
            {
                (R as Spell.Skillshot).CastIfItWillHit(2);
                //R.CastIfWillHit(target, 2);
            }
        }

        public override void setUpSpells()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 650, EloBuddy.SDK.Enumerations.SkillShotType.Linear, 250, 180, 850);
            W = new Spell.Active(SpellSlot.W, 1500);
            E = new Spell.Active(SpellSlot.E, 400);
            R = new Spell.Skillshot(SpellSlot.R, 1420, EloBuddy.SDK.Enumerations.SkillShotType.Linear, 250, 300, 4500);
        }

        public override void useSpells()
        {
            var tar = ARAMTargetSelector.getBestTarget(Q.Range);
            if (tar != null) useQ(tar);
            tar = ARAMTargetSelector.getBestTarget(W.Range+200);
            if (tar != null)
                useW(tar);
            else if(HasPassive())
                W.Cast();

            tar = ARAMTargetSelector.getBestTarget(R.Range);
            if (tar != null) useR(tar);
        }

        private bool HasPassive()
        {
            return player.HasBuff("AurelionSolWActive");
        }
    }
}
