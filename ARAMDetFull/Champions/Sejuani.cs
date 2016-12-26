using System.Collections.Generic;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;

namespace ARAMDetFull.Champions
{
    class Sejuani : Champion
    {
        public Sejuani()
        {
            ARAMSimulator.champBuild = new Build
            {
                coreItems = new List<ConditionalItem>
                {
                    new ConditionalItem(ItemId.Sunfire_Cape),
                    new ConditionalItem(ItemId.Mercurys_Treads,ItemId.Ninja_Tabi,ItemCondition.ENEMY_AP),
                    new ConditionalItem(ItemId.Abyssal_Scepter),
                    new ConditionalItem(ItemId.Banner_of_Command,ItemId.Locket_of_the_Iron_Solari,ItemCondition.ENEMY_AP),
                    new ConditionalItem(ItemId.Warmogs_Armor),
                    new ConditionalItem(ItemId.Spirit_Visage,ItemId.Iceborn_Gauntlet,ItemCondition.ENEMY_AP),
                },
                startingItems = new List<ItemId>
                {
                    ItemId.Bamis_Cinder
                }
            };
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
            if (!W.IsReady() || target == null)
                return;
            W.Cast();
        }

        public override void useE(Obj_AI_Base target)
        {
            if (!E.IsReady() || target == null)
                return;
            foreach (var enemy in ObjectManager.Get<AIHeroClient>().Where(enemy => enemy.IsEnemy && !enemy.IsDead))
            {
                if (enemy.HasBuff("SejuaniFrost") && E.IsReady())
                {
                    E.Cast();
                }
            }
        }

        private void AutoR()
        {
            foreach (var enemy in ObjectManager.Get<AIHeroClient>().Where(enemy => enemy.IsEnemy && !enemy.IsDead && enemy.Position.CountEnemiesInRange(350) >= 2))
            {
                R.Cast(enemy);
            }
        }

        public override void useR(Obj_AI_Base target)
        {
            if (!R.IsReady() || target == null)
                return;
            AutoR();
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
            Q = new Spell.Skillshot(SpellSlot.Q, 650, SkillShotType.Linear, 0, 1600, 70)
            {
                AllowedCollisionCount = -1
            };
            W = new Spell.Active(SpellSlot.W, 350);
            E = new Spell.Active(SpellSlot.E, 1000);
            R = new Spell.Skillshot(SpellSlot.R, 1200, SkillShotType.Linear, 250, 1600, 110)
            {
                AllowedCollisionCount = -1
            };
        }
    }
}