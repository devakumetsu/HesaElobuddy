using System.Collections.Generic;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;

namespace ARAMDetFull.Champions
{
    class Tristana : Champion
    {
        public Tristana()
        {
            ARAMSimulator.champBuild = new Build
            {
                coreItems = new List<ConditionalItem>
                {
                    new ConditionalItem(ItemId.Infinity_Edge),
                    new ConditionalItem(ItemId.Berserkers_Greaves),
                    new ConditionalItem(ItemId.Runaans_Hurricane_Ranged_Only),
                    new ConditionalItem((ItemId)3094),
                    new ConditionalItem(ItemId.Blade_of_the_Ruined_King),
                    new ConditionalItem(ItemId.Guinsoos_Rageblade),
                },
                startingItems = new List<ItemId>
                {
                    ItemId.Pickaxe,ItemId.Boots_of_Speed
                }
            };
        }

        public override void useQ(Obj_AI_Base target)
        {
            if (!Q.IsReady())
                return;
            Q.Cast();
            Aggresivity.addAgresiveMove(new AgresiveMove(90));
            Player.IssueOrder(GameObjectOrder.AttackUnit, target);
        }

        public override void useW(Obj_AI_Base target)
        {
            if (!W.IsReady())
                return;

            if (EnemyInRange(2, 500) || (EnemyInRange(1, 500) && player.HealthPercent < 35))
                W.Cast(Player.Instance.Position.Extend(ARAMSimulator.fromNex.Position, 900).To3D());
        }

        public override void useE(Obj_AI_Base target)
        {
            if (!E.IsReady())
                return;
            E.Cast(target);
            if (target.HasBuff("TristanaECharge"))
            {
                Player.IssueOrder(GameObjectOrder.AttackUnit, target);
            }
        }

        public override void useR(Obj_AI_Base target)
        {
            if (!R.IsReady())
                return;
            if (R.GetDamage(target) > target.Health || (player.HealthPercent < 40 || target.Distance(player) < 400))
                R.CastOnUnit(target);
        }

        public override void setUpSpells()
        {
            //New Spells /updated
            Q = new Spell.Active(SpellSlot.Q);
            W = new Spell.Skillshot(SpellSlot.W, 900, SkillShotType.Circular, 400, 1400, 150);
            E = new Spell.Targeted(SpellSlot.E, 600);
            R = new Spell.Targeted(SpellSlot.R, 600);
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

        public static bool EnemyInRange(int numOfEnemy, float range)
        {
            return Player.Instance.CountEnemiesInRange((int)range) >= numOfEnemy;
        }
    }
}