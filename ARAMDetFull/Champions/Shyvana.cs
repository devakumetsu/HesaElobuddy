using System.Collections.Generic;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;

namespace ARAMDetFull.Champions
{
    class Shyvana : Champion
    {
        public Shyvana()
        {
            ARAMSimulator.champBuild = new Build
            {
                coreItems = new List<ConditionalItem>
                {
                    new ConditionalItem(ItemId.Blade_of_the_Ruined_King),
                    new ConditionalItem(ItemId.Mercurys_Treads, ItemId.Ninja_Tabi, ItemCondition.ENEMY_AP),
                    new ConditionalItem(ItemId.Spirit_Visage, ItemId.Randuins_Omen, ItemCondition.ENEMY_AP),
                    new ConditionalItem(ItemId.Sunfire_Cape),
                    new ConditionalItem(ItemId.Ravenous_Hydra_Melee_Only),
                    new ConditionalItem(ItemId.Banshees_Veil),
                },
                startingItems = new List<ItemId>
                {
                    ItemId.Vampiric_Scepter,
                    ItemId.Boots_of_Speed
                }
            };
        }

        public override void useQ(Obj_AI_Base target)
        {
            if (!Q.IsReady())
                return;
            Q.CastOnUnit(target);
        }

        public override void useW(Obj_AI_Base target)
        {
            if (!W.IsReady())
                return;
            if (safeGap(target))
                W.CastOnUnit(target);
        }

        public override void useE(Obj_AI_Base target)
        {
            if (!E.IsReady() || W.IsReady())
                return;
            E.Cast(target);
        }

        public override void useR(Obj_AI_Base target)
        {
            return;
            if (!R.IsReady())
                return;
            if (player.Path.Length > 0 && player.Path[player.Path.Length - 1].Distance(player.Position) > 2500)
            {
                R.Cast(player.Path[player.Path.Length - 1]);
            }


        }

        public override void setUpSpells()
        {
            Q = new Spell.Active(spellSlot: SpellSlot.Q, spellRange: 650);
            W = new Spell.Active(spellSlot: SpellSlot.W, spellRange: 325);
            E = new Spell.Skillshot(spellSlot: SpellSlot.E, spellRange: 925, skillShotType: SkillShotType.Linear, spellSpeed: 1200, spellWidth: 60);
            R = new Spell.Skillshot(spellSlot: SpellSlot.R, spellRange: 850, skillShotType: SkillShotType.Linear, spellSpeed: 700, spellWidth: 160);

        }

        public override void useSpells()
        {
            var tar = ARAMTargetSelector.getBestTarget(600);
            if (tar == null)
                return;
            useQ(tar);
            useW(tar);
            useE(tar);
            useR(tar);
        }
    }
}