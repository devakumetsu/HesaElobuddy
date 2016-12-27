using System;
using System.Collections.Generic;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;

namespace ARAMDetFull.Champions
{
    class Varus : Champion
    {
        public static Spell.Chargeable Q;

        public Varus()
        {
            Orbwalker.OnPreAttack += BeforeAttack;
            ARAMSimulator.champBuild = new Build
            {
                coreItems = new List<ConditionalItem>
                {
                    new ConditionalItem(ItemId.The_Bloodthirster),
                    new ConditionalItem(ItemId.Berserkers_Greaves),
                    new ConditionalItem(ItemId.Infinity_Edge),
                    new ConditionalItem(ItemId.Phantom_Dancer),
                    new ConditionalItem(ItemId.Blade_of_the_Ruined_King),
                    new ConditionalItem(ItemId.Banshees_Veil),
                },
                startingItems = new List<ItemId>
                {
                    ItemId.Vampiric_Scepter,ItemId.Boots_of_Speed
                }
            };
        }

        private void BeforeAttack(AttackableUnit unit, Orbwalker.PreAttackArgs args)
        {
            args.Process = !Q.IsCharging;
        }
        
        public override void useQ(Obj_AI_Base target)
        {
            if (Q.IsCharging)
            {
                Console.WriteLine("Check targets!!");
                var prediction = Q.GetPrediction(target);
                if (prediction.HitChance >= HitChance.High)
                {
                    Console.WriteLine("Relase!!");
                    Q.Cast(prediction.CastPosition);
                }
            }
            if (Q.IsReady())
            {

                if (!Q.IsCharging)
                {
                    Q.StartCharging();
                    return;
                }
            }
        }

        public override void useW(Obj_AI_Base target)
        {
            //  if (!W.IsReady())
            //      return;
            //  W.Cast(target);
        }

        public override void useE(Obj_AI_Base target)
        {
            if (!E.IsReady() || player.Spellbook.IsChanneling)
                return;
            E.Cast(target);

        }

        public override void useR(Obj_AI_Base target)
        {
            if (!R.IsReady() || player.Spellbook.IsChanneling)
                return;
            R.Cast(target);
        }

        public override void setUpSpells()
        {
            Q = new Spell.Chargeable(SpellSlot.Q, 1000, 1600, 1300, 0, 1900, 70)
            {
                AllowedCollisionCount = int.MaxValue
            };
            W = new Spell.Active(SpellSlot.W);
            E = new Spell.Skillshot(SpellSlot.E, 925, SkillShotType.Circular, 250, 1500, 235);
            R = new Spell.Skillshot(SpellSlot.R, 1250, SkillShotType.Linear, 250, 1950, 120)
            {
                AllowedCollisionCount = -1
            };
        }

        public override void useSpells()
        {
            var tar = ARAMTargetSelector.getBestTarget(Q.Range);
            if (tar != null) useQ(tar);
            tar = ARAMTargetSelector.getBestTarget(E.Range);
            if (tar != null) useE(tar);
            tar = ARAMTargetSelector.getBestTarget(R.Range);
            if (tar != null) useR(tar);
        }
    }
}