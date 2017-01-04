using System.Collections.Generic;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;

namespace ARAMDetFull.Champions
{
    class Sona : Champion
    {
        public Sona()
        {
            Interrupter.OnInterruptableSpell += InterrupterOnOnPossibleToInterrupt;

            ARAMSimulator.champBuild = new Build
            {
                coreItems = new List<ConditionalItem>
                {
                    new ConditionalItem(ItemId.Athenes_Unholy_Grail),
                    new ConditionalItem(ItemId.Sorcerers_Shoes),
                    new ConditionalItem(ItemId.Rabadons_Deathcap),
                    new ConditionalItem(ItemId.Void_Staff),
                    new ConditionalItem(ItemId.Ludens_Echo),
                    new ConditionalItem(ItemId.Spirit_Visage),
                },
                startingItems = new List<ItemId>
                {
                    ItemId.Chalice_of_Harmony,
                    ItemId.Boots_of_Speed
                }
            };
        }



        private void InterrupterOnOnPossibleToInterrupt(Obj_AI_Base sender, Interrupter.InterruptableSpellEventArgs args)
        {
            var unit = sender;
            var spell = args;

            if (!unit.IsValidTarget(R.Range))
            {
                return;
            }
            if (!R.IsReady())
            {
                return;
            }

            R.Cast(unit);
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
                W.Cast(target);
            }
        }

        public override void useE(Obj_AI_Base target)
        {

        }


        public override void useR(Obj_AI_Base target)
        {
            if (R.IsReady())
                R.CastIfWillHit(target, 2);
        }

        public override void useSpells()
        {
            AutoW();
            var tar = ARAMTargetSelector.getBestTarget(Q.Range);
            if (tar != null) useQ(tar);
            tar = ARAMTargetSelector.getBestTarget(W.Range);
            //if (tar != null) useW(tar);
            tar = ARAMTargetSelector.getBestTarget(E.Range);
            if (tar != null) useE(tar);
            tar = ARAMTargetSelector.getBestTarget(R.Range);
            if (tar != null) useR(tar);

        }

        public override void setUpSpells()
        {
            //Create the spells
            Q = new Spell.Active(SpellSlot.Q, 850);
            W = new Spell.Active(SpellSlot.W, 1000);
            E = new Spell.Active(SpellSlot.E, 350);
            R = new Spell.Skillshot(SpellSlot.R, 1000, SkillShotType.Linear, 500, 125, 3000)
            {
                AllowedCollisionCount = int.MaxValue
            };
        }

        private void AutoW()
        {
            if (!W.IsReady())
            {
                return;
            }

            foreach (var friend in
                EntityManager.Heroes.Allies.Where(x => x.IsAlly).Where(x => !x.IsDead).Where(x => !x.IsZombie))
            {
                var health = 35;

                if (friend.HealthPercent <= health)
                {
                    W.Cast();
                }
            }
        }
    }
}