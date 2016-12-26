using System.Collections.Generic;
using EloBuddy;
using EloBuddy.SDK.Events;
using EloBuddy.SDK;

namespace ARAMDetFull.Champions
{
    class XinZhao : Champion
    {
        public XinZhao()
        {
            Interrupter.OnInterruptableSpell += Interrupter_OnInterruptableSpell;

            ARAMSimulator.champBuild = new Build
            {
                coreItems = new List<ConditionalItem>
                {
                    new ConditionalItem(ItemId.Blade_of_the_Ruined_King),
                    new ConditionalItem(ItemId.Mercurys_Treads, ItemId.Ninja_Tabi, ItemCondition.ENEMY_AP),
                    new ConditionalItem(ItemId.Youmuus_Ghostblade),
                    new ConditionalItem(ItemId.Spirit_Visage),
                    new ConditionalItem(ItemId.Ravenous_Hydra_Melee_Only),
                    new ConditionalItem(ItemId.Banshees_Veil),
                },
                startingItems = new List<ItemId>
                {
                    ItemId.Vampiric_Scepter,
                    ItemId.Boots_of_Speed
                }
            };
        }

        private void Interrupter_OnInterruptableSpell(Obj_AI_Base unit, Interrupter.InterruptableSpellEventArgs e)
        {
            if (e.DangerLevel != EloBuddy.SDK.Enumerations.DangerLevel.High)
            {
                return;
            }
            if (R.IsReady() && R.IsInRange(unit))
            {
                R.Cast();
            }
        }

        public override void useQ(Obj_AI_Base target)
        {
            if (!Q.IsReady())
                return;

             Q.Cast();
        }

        public override void useW(Obj_AI_Base target)
        {
            if (!W.IsReady())
                return;
            W.Cast();
        }

        public override void useE(Obj_AI_Base target)
        {
            if (!E.IsReady())
                return;
            if (!target.IsUnderHisturret() && (MapControl.balanceAroundPoint(target.Position.To2D(), 700) >= -1 || (MapControl.fightIsOn() != null && MapControl.fightIsOn().NetworkId == target.NetworkId)))
                E.Cast(target);
        }

        public override void useR(Obj_AI_Base target)
        {
            if (!R.IsReady())
                return;
            if (player.CountEnemiesInRange(400) > 1)
                R.Cast();
        }

        public override void setUpSpells()
        {
            //Initialize spells.
            Q = new Spell.Active(SpellSlot.Q);
            W = new Spell.Active(SpellSlot.W);
            E = new Spell.Targeted(SpellSlot.E, 650);
            R = new Spell.Active(SpellSlot.R, 500);
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
    }
}