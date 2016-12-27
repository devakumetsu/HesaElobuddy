using System.Collections.Generic;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;

namespace ARAMDetFull.Champions
{
    class Urgot : Champion
    {
        private Spell.Skillshot Q2;

        public Urgot()
        {
            ARAMSimulator.champBuild = new Build
            {
                coreItems = new List<ConditionalItem>
                {
                    new ConditionalItem(ItemId.The_Black_Cleaver),
                    new ConditionalItem(ItemId.Mercurys_Treads),
                    new ConditionalItem(ItemId.Last_Whisper),
                    new ConditionalItem(ItemId.The_Bloodthirster),
                    new ConditionalItem(ItemId.Frozen_Heart),
                    new ConditionalItem(ItemId.Banshees_Veil),
                },
                startingItems = new List<ItemId>
                {
                    ItemId.Phage
                }
            };
        }

        private void SmartQ()
        {
            if (!Q.IsReady())
            {
                return;
            }

            foreach (var obj in
                ObjectManager.Get<AIHeroClient>()
                    .Where(obj => obj.IsValidTarget(Q2.Range) && obj.HasBuff("urgotcorrosivedebuff")))
            {
                W.Cast();
                Q2.Cast(obj.ServerPosition);
            }
        }

        public override void useQ(Obj_AI_Base target)
        {
            if (target == null)
                return;
            if (Q.IsReady() &&
                target.IsValidTarget(target.HasBuff("urgotcorrosivedebuff") ? Q2.Range : Q.Range))
            {
                Q.Cast(target.ServerPosition);
            }
        }

        public override void useW(Obj_AI_Base target)
        {
            if (!W.IsReady() || target == null)
                return;
            if (target.IsValidTarget(300))
                W.Cast(target);
        }

        public override void useE(Obj_AI_Base target)
        {
            if (!E.IsReady() || target == null)
            {
                return;
            }
            var hitchance = (HitChance.Medium);
            if (target.IsValidTarget(E.Range))
            {
                E.CastIfHitchanceEquals(target, hitchance, true);
            }
            else
            {
                var tar = ARAMTargetSelector.getBestTarget(E.Range);
                if (tar != null)
                    E.CastIfHitchanceEquals(tar, HitChance.High, true);
            }
        }

        public override void useR(Obj_AI_Base target)
        {
            if (!R.IsReady() || target == null)
                return;
            if (safeGap(target))
                R.CastOnUnit(target);
        }

        public override void useSpells()
        {
            SmartQ();
            var tar = ARAMTargetSelector.getBestTarget(Q.Range);
            if (tar != null) useQ(tar);
            tar = ARAMTargetSelector.getBestTarget(450);
            useW(tar);
            tar = ARAMTargetSelector.getBestTarget(E.Range + 100);
            if (tar != null) useE(tar);
            var target = ARAMTargetSelector.getBestTarget(R.Range);
            if (target != null) useR(target);
        }

        public override void setUpSpells()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 900, SkillShotType.Linear, 150, 1600, 60)
            {
                AllowedCollisionCount = 0
            };
            Q2 = new Spell.Skillshot(SpellSlot.Q, 1200, SkillShotType.Linear, 100, 1600, 100);
            W = new Spell.Active(SpellSlot.W);
            E = new Spell.Skillshot(SpellSlot.E, 900, SkillShotType.Circular, 250, 1500, 250)
            {
                AllowedCollisionCount = int.MaxValue
            };
            R = new Spell.Targeted(SpellSlot.R, 500);
        }
    }
}