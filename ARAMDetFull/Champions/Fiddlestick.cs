using System.Collections.Generic;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;

namespace ARAMDetFull.Champions
{
    class Fiddlestick : Champion
    {
        public int DrainStart = 0;
        
        public bool JustUsedDrain
        {
            get { return DrainStart + 500 > Core.GameTickCount; }
        }

        public Fiddlestick()
        {
            ARAMSimulator.champBuild = new Build
            {
                coreItems = new List<ConditionalItem>
                {
                    new ConditionalItem(ItemId.Zhonyas_Hourglass),
                    new ConditionalItem(ItemId.Ionian_Boots_of_Lucidity),
                    new ConditionalItem(ItemId.Morellonomicon),
                    new ConditionalItem(ItemId.Rabadons_Deathcap),
                    new ConditionalItem(ItemId.Abyssal_Scepter),
                    new ConditionalItem(ItemId.Void_Staff),
                },
                startingItems = new List<ItemId>
                {
                    ItemId.Needlessly_Large_Rod
                }
            };
        }

        public override void useQ(Obj_AI_Base target)
        {
            if (target == null || !Q.IsReady())
                return;
            Q.CastOnUnit(target);
        }

        public override void useW(Obj_AI_Base target)
        {
            if (!W.IsReady() || target == null || Q.IsReady() || E.IsReady())
                return;
            W.CastOnUnit(target);
        }

        public override void useE(Obj_AI_Base target)
        {
            if (!E.IsReady() || target == null) return;
            if (E.Cast(target))
                DrainStart = Core.GameTickCount;
        }

        public override void useR(Obj_AI_Base target)
        {
            if (!R.IsReady())
                return;
            if (!Sector.inTowerRange(target.Position.To2D()) && (MapControl.fightIsOn() != null) &&
                player.HealthPercent > 45)
            {
                R.Cast(target.Position);
                Aggresivity.addAgresiveMove(new AgresiveMove(400, 5000, true));
            }
        }

        public override void useSpells()
        {
            if (player.HasBuff("Drain") || JustUsedDrain)
            {
                Orbwalker.DisableMovement = true;
                Orbwalker.DisableAttacking = true;
            }
            if (JustUsedDrain)
                return;
            if (!player.HasBuff("Drain"))
            {
                Orbwalker.DisableMovement = false;
                Orbwalker.DisableAttacking = false;
            }

            var tar = ARAMTargetSelector.getBestTarget(Q.Range);
            if (tar != null) useQ(tar);
            tar = ARAMTargetSelector.getBestTarget(E.Range);
            if (tar != null) useE(tar);
            tar = ARAMTargetSelector.getBestTarget(W.Range);
            if (tar != null) useW(tar);
            tar = ARAMTargetSelector.getBestTarget(R.Range);
            if (tar != null) useR(tar);
        }

        public override void setUpSpells()
        {
            Q = new Spell.Targeted(SpellSlot.Q, 575);
            W = new Spell.Targeted(SpellSlot.W, 575);
            E = new Spell.Targeted(SpellSlot.E, 750);
            R = new Spell.Skillshot(SpellSlot.R, 800, SkillShotType.Circular);
        }
    }
}