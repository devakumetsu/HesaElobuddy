using System.Collections.Generic;
using EloBuddy;
using EloBuddy.SDK;
using System;
using EloBuddy.SDK.Enumerations;

namespace ARAMDetFull.Champions
{
    class Pantheon : Champion
    {
        public Pantheon()
        {
            ARAMSimulator.champBuild = new Build
            {
                coreItems = new List<ConditionalItem>
                {
                    new ConditionalItem(ItemId.The_Black_Cleaver),
                    new ConditionalItem(ItemId.Mercurys_Treads,ItemId.Ninja_Tabi,ItemCondition.ENEMY_AP),
                    new ConditionalItem(ItemId.Ravenous_Hydra_Melee_Only),
                    new ConditionalItem(ItemId.Maw_of_Malmortius),
                    new ConditionalItem(ItemId.Sunfire_Cape),
                    new ConditionalItem(ItemId.Banshees_Veil),
                },
                startingItems = new List<ItemId>
                {
                    ItemId.Phage
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
            if(!Sector.inTowerRange(target.Position.To2D()) && (MapControl.balanceAroundPoint(target.Position.To2D(),700)>=-1 || (MapControl.fightIsOn() != null && MapControl.fightIsOn().NetworkId == target.NetworkId))  )
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
            return;//TODO: Test why this is disabled.
            if(!R.IsReady())
                return;
            if (player.Path.Length > 0 && player.Path[player.Path.Length-1].Distance(player.Position)>2500)
            {
                R.Cast(player.Path[player.Path.Length - 1]);
            }
        }

        public override void setUpSpells()
        {
            Q = new Spell.Targeted(SpellSlot.Q, 600);
            W = new Spell.Targeted(SpellSlot.W, 600);
            E = new Spell.Skillshot(SpellSlot.E, 400, SkillShotType.Cone, 250, 800, (int)(35 * Math.PI / 180));
            R = new Spell.Skillshot(SpellSlot.R, 2000, SkillShotType.Circular) {
                AllowedCollisionCount = int.MaxValue
            };
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
