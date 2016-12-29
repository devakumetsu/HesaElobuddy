using System;
using System.Collections.Generic;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using SharpDX;

namespace ARAMDetFull.Champions
{
    class Alistar : Champion
    {
        public Alistar()
        {
            ARAMSimulator.champBuild = new Build
            {
                coreItems = new List<ConditionalItem>
                {
                    new ConditionalItem(ItemId.Locket_of_the_Iron_Solari),
                    new ConditionalItem(ItemId.Boots_of_Mobility),
                    new ConditionalItem(ItemId.Randuins_Omen),
                    new ConditionalItem(ItemId.Banshees_Veil),
                    new ConditionalItem(ItemId.Warmogs_Armor),
                    new ConditionalItem(ItemId.Guardian_Angel),
                },
                startingItems = new List<ItemId>
                {
                    ItemId.Aegis_of_the_Legion
                }
            };
        }

        public override void useQ(Obj_AI_Base target)
        {
            if (Q.IsReady() && Q.IsInRange(target))
            {
                Q.Cast(target);
            }
        }

        public override void useW(Obj_AI_Base target)
        {
            if (W.IsReady())
            {
             //   if (Player.Instance.CountEnemiesInRange(W.Range) <= 1)
                {
                    //W.Cast(target);
                }
            }
        }

        public override void useE(Obj_AI_Base alliet)
        {
            if (E.IsReady())
            {
                if (player.ManaPercent <= 30)
                {
                    E.Cast();
                }
            }
        }


        public override void useR(Obj_AI_Base target)
        {
            if (R.IsReady())
            {
                if (player.HealthPercent >= 20 && Player.Instance.CountEnemiesInRange(500) <= 2)
                    R.Cast();
            }
        }

        public override void useSpells()
        {
            var tar = ARAMTargetSelector.getBestTarget(Q.Range);
            if (tar != null) useQ(tar);
            tar = ARAMTargetSelector.getBestTarget(W.Range);
            if (tar != null) useW(tar);
            tar = ARAMTargetSelector.getBestTarget(E.Range);
            if (tar != null) useE(tar);
            //tar = ARAMTargetSelector.getBestTarget(R.Range);
            if (tar != null) useR(tar);

        }

        public override void setUpSpells()
        {
            //Create the spells
            Q = new Spell.Active(SpellSlot.Q, 365);
            W = new Spell.Targeted(SpellSlot.W, 650);
            E = new Spell.Active(SpellSlot.E, 575);
            R = new Spell.Active(SpellSlot.R);
        }
    }
}
