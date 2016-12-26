using System.Collections.Generic;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;

namespace ARAMDetFull.Champions
{
    class Ezreal : Champion
    {
        public Ezreal()
        {
            ARAMSimulator.champBuild = new Build
            {
                coreItems = new List<ConditionalItem>
                {
                    new ConditionalItem(ItemId.Trinity_Force),
                    new ConditionalItem(ItemId.Berserkers_Greaves),
                    new ConditionalItem(ItemId.Infinity_Edge),
                    new ConditionalItem(ItemId.The_Bloodthirster),
                    new ConditionalItem(ItemId.Last_Whisper),
                    new ConditionalItem(ItemId.Banshees_Veil,ItemId.Zhonyas_Hourglass,ItemCondition.ENEMY_AP),
                },
                startingItems = new List<ItemId>
                {
                    ItemId.Sheen
                }
            };
        }

        public override void useQ(Obj_AI_Base target)
        {
            if (!Q.IsReady())
                return;
            Q.Cast(target);
        }

        public override void useW(Obj_AI_Base target)
        {
            if (!W.IsReady() || player.ManaPercent < 45)
                return;
            W.Cast(target);
        }

        public override void useE(Obj_AI_Base target)
        {
            if (!E.IsReady())
                return;
            if (EnemyInRange(1, 300) || (EnemyInRange(1, 600) && player.HealthPercent < 30))
                E.Cast(player.Position.Extend(ARAMSimulator.fromNex.Position, 400).To3D());
        }

        public override void useR(Obj_AI_Base target)
        {
            if (!R.IsReady())
                return;
            if (target.Health < R.GetDamage(target))
                R.Cast(target);
            else
                R.CastIfWillHit(target, 2);
        }

        public override void setUpSpells()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 1200, SkillShotType.Linear, 250, 2000, 60);
            //Q.SetSkillshot(0.25f, 60f, 2000f, true, SkillshotType.SkillshotLine);

            W = new Spell.Skillshot(SpellSlot.W, 950, SkillShotType.Linear, 250, 1600, 80);
            //W.SetSkillshot(0.25f, 80f, 1600f, false, SkillshotType.SkillshotLine);

            E = new Spell.Active(SpellSlot.E, 400);

            R = new Spell.Skillshot(SpellSlot.R, 2200, SkillShotType.Linear, 1000, 2000, 160);
            //R.SetSkillshot(1f, 160f, 2000f, false, SkillshotType.SkillshotLine);
        }

        public override void useSpells()
        {
            var tar = ARAMTargetSelector.getBestTarget(1010);
            if (tar != null) useQ(tar);
            tar = ARAMTargetSelector.getBestTarget(500);
            if (tar != null) useE(tar);
            tar = ARAMTargetSelector.getBestTarget(W.Range);
            if (tar != null) useW(tar);
            tar = ARAMTargetSelector.getBestTarget(R.Range);
            if (tar != null) useR(tar);
        }

        public static bool EnemyInRange(int numOfEnemy, float range)
        {
            return Player.Instance.CountEnemiesInRange((int)range) >= numOfEnemy;
        }

        public override void farm()
        {
            if (player.ManaPercent < 55 || !Q.IsReady())
                return;
            var lanemonster = EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy, player.ServerPosition, Q.Range - 50);
            foreach (var minion in lanemonster)
            {
                if (minion.Health > ObjectManager.Player.GetAutoAttackDamage(minion) && minion.Health < Q.GetDamage(minion))
                {
                    Q.Cast(minion);
                    return;
                }
            }
        }
    }
}