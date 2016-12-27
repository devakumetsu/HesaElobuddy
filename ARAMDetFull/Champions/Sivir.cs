using System;
using System.Collections.Generic;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using SharpDX;

namespace ARAMDetFull.Champions
{
    class Sivir : Champion
    {
        public Sivir()
        {
            ARAMSimulator.champBuild = new Build
            {
                coreItems = new List<ConditionalItem>
                {
                    new ConditionalItem(ItemId.Infinity_Edge),
                    new ConditionalItem(ItemId.Berserkers_Greaves),
                    new ConditionalItem(ItemId.Phantom_Dancer),
                    new ConditionalItem(ItemId.Lord_Dominiks_Regards),
                    new ConditionalItem(ItemId.Essence_Reaver),
                    new ConditionalItem((ItemId)3036),
                },
                startingItems = new List<ItemId>
                {
                    ItemId.Pickaxe,ItemId.Boots_of_Speed
                }
            };
        }

        public override void useQ(Obj_AI_Base target)
        {
            if (Q.IsReady())
            {
                Q.Cast(target);
            }
        }

        public override void useW(Obj_AI_Base target)
        {
            if (W.IsReady())
            {
                if (Player.Instance.CountEnemiesInRange(W.Range) <= 1)
                {
                    W.Cast();
                }
            }
        }

        public override void useE(Obj_AI_Base target)
        {
            //if (E.IsReady())
              //  E.Cast(target);
        }


        public override void useR(Obj_AI_Base target)
        {
            if (R.IsReady())
            {
                if (Player.Instance.CountAlliesInRange(R.Range) <=3  && Player.Instance.CountEnemiesInRange(500) <=2)
                    R.Cast();
            }
        }

        public override void useSpells()
        {
            var tar = ARAMTargetSelector.getBestTarget(Q.Range);
            if (tar != null) useQ(tar);
            tar = ARAMTargetSelector.getBestTarget(W.Range);
            //if (tar != null) useW(tar);
            if (tar != null) useW(tar);
            //if (tar != null) useE(tar);
            if (tar != null) useR(tar);

        }

        public override void setUpSpells()
        {
            //Create the spells
            Q = new Spell.Skillshot(SpellSlot.Q, 1200, SkillShotType.Linear, 250, 1350, 90)
            {
                AllowedCollisionCount = int.MaxValue
            };
            W = new Spell.Active(SpellSlot.W);
            E = new Spell.Active(SpellSlot.E,20);
            R = new Spell.Active(SpellSlot.R, 1000);
        }
    }
}
