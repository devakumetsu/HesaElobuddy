using System.Collections.Generic;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;

namespace ARAMDetFull.Champions
{
    class Illaoi : Champion
    {
        public Illaoi()
        {
            ARAMSimulator.champBuild = new Build
            {
                coreItems = new List<ConditionalItem>
                {
                    new ConditionalItem(ItemId.The_Black_Cleaver),
                    new ConditionalItem(ItemId.Mercurys_Treads,ItemId.Ninja_Tabi,ItemCondition.ENEMY_AP),
                    new ConditionalItem(ItemId.Spirit_Visage),
                    new ConditionalItem(ItemId.Warmogs_Armor),
                    new ConditionalItem(ItemId.Steraks_Gage),
                    new ConditionalItem(ItemId.Deaths_Dance, ItemId.Titanic_Hydra, ItemCondition.ENEMY_LOSING),
                },
                startingItems = new List<ItemId>
                {
                    ItemId.Caulfields_Warhammer
                }
            };
        }

        public override void useQ(Obj_AI_Base target)
        {
            if (!Q.IsReady() || target == null)
                return;
            var enemy = TargetSelector.GetTarget(Q.Range, DamageType.Physical);
            var enemyGhost = ObjectManager.Get<Obj_AI_Minion>().FirstOrDefault(x => x.Name == enemy.Name);
            if (enemy != null && enemyGhost == null)
            {
                if (Q.CanCast(enemy) && Q.GetPrediction(enemy).HitChance >= HitChance.High && Q.GetPrediction(enemy).CollisionObjects.Count() == 0)
                {
                    Q.Cast(enemy);
                }
            }
            if (enemy == null && enemyGhost != null)
            {
                if (Q.CanCast(enemyGhost) && Q.GetPrediction(enemyGhost).HitChance >= HitChance.High && Q.GetPrediction(enemyGhost).CollisionObjects.Count() == 0)
                {
                    Q.Cast(enemyGhost);
                }
            }
        }

        public override void useW(Obj_AI_Base target)
        {
            if (!W.IsReady() || target == null)
                return;
            W.Cast();
            Aggresivity.addAgresiveMove(new AgresiveMove(160, 800, true));
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
            if (player.CountEnemiesInRange(450) >= 2)
            {
                R.Cast(target.Position);
                Aggresivity.addAgresiveMove(new AgresiveMove(1200, 4500));
            }
        }

        public override void useSpells()
        {
            var tar = ARAMTargetSelector.getBestTarget(E.Range);
            if (tar != null) useE(tar);
            tar = ARAMTargetSelector.getBestTarget(R.Range);
            if (tar != null) useR(tar);
            tar = ARAMTargetSelector.getBestTarget(W.Range);
            if (tar != null) useW(tar);
            tar = ARAMTargetSelector.getBestTarget(Q.Range);
            if (tar != null) useQ(tar);
        }
        
        public override void setUpSpells()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 850, SkillShotType.Circular, 484, 500, 0);
            W = new Spell.Targeted(SpellSlot.W, 600);
            E = new Spell.Skillshot(SpellSlot.E, 900, SkillShotType.Linear, 66, 1900, 50);
            R = new Spell.Active(SpellSlot.R, 450);
        }
    }
}