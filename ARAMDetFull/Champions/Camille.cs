using System;
using System.Collections.Generic;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using SharpDX;

namespace ARAMDetFull.Champions
{
    class Camille : Champion
    {
        public Spell.Active Q2 { get; private set; }

        public Camille()
        {
            ARAMSimulator.champBuild = new Build
            {
                coreItems = new List<ConditionalItem>
                {
                    new ConditionalItem(ItemId.Titanic_Hydra),
                    new ConditionalItem(ItemId.Mercurys_Treads),
                    new ConditionalItem(ItemId.Randuins_Omen),
                    new ConditionalItem(ItemId.Spirit_Visage),
                    new ConditionalItem(ItemId.The_Black_Cleaver),
                    new ConditionalItem(ItemId.Banshees_Veil),
                },
                startingItems = new List<ItemId>
                {
                    ItemId.Tiamat
                }
            };
        }

        public override void useQ(Obj_AI_Base target)
        {
            if (Q.IsReady())
            {
                Q.Cast();
                Player.IssueOrder(GameObjectOrder.AutoAttack, target);
                Q2.Cast();
                Player.IssueOrder(GameObjectOrder.AutoAttack, target);
            }
        }

        public override void useW(Obj_AI_Base target)
        {
            if (W.IsReady())
                W.Cast(target);
        }

        public override void useE(Obj_AI_Base target)
        {
            if (Player.Instance.HasItem(ItemId.Titanic_Hydra))
            {
                Item.UseItem(ItemId.Titanic_Hydra);
            }
        }

        public override void useR(Obj_AI_Base target)
        {
            if (R.IsReady())
            {
                if (target.HealthPercent >40 && player.CountEnemiesInRange(500) <=1)
                {
                    R.Cast(target);
                }
            }
        }

        public override void useSpells()
        {
            var tar = ARAMTargetSelector.getBestTarget(Q.Range);
            if (tar != null) useQ(tar);
            tar = ARAMTargetSelector.getBestTarget(W.Range);
            if (tar != null) useW(tar);
            tar = ARAMTargetSelector.getBestTarget(Q.Range);
            if (tar != null) useE(tar);
            tar = ARAMTargetSelector.getBestTarget(R.Range);
            if (tar != null) useR(tar);

        }

        public override void setUpSpells()
        {
            //Create the spells
            Q = new Spell.Active(SpellSlot.Q);
            Q2 = new Spell.Active(SpellSlot.Q);
            W = new Spell.Skillshot(SpellSlot.W, 600, EloBuddy.SDK.Enumerations.SkillShotType.Cone, 250, int.MaxValue, 50, DamageType.Physical)
            {
                ConeAngleDegrees = 45,
            };
            /*W2 = new Spell.Skillshot(SpellSlot.W, 325, EloBuddy.SDK.Enumerations.SkillShotType.Cone, 250, int.MaxValue, 50, DamageType.Physical)
            {
                ConeAngleDegrees = 45,
            };*/
            E = new Spell.Skillshot(SpellSlot.E, 1100, EloBuddy.SDK.Enumerations.SkillShotType.Linear, 250, 1000, 80);
            R = new Spell.Targeted(SpellSlot.R, 475);
        }
    }
}