using System.Collections.Generic;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;

namespace ARAMDetFull.Champions
{
    class Nautilus : Champion
    {
        public int AllowedCollisionCount { get; private set; }

        public Nautilus()
        {
            Interrupter.OnInterruptableSpell += InterrupterOnOnPossibleToInterrupt;

            ARAMSimulator.champBuild = new Build
            {
                coreItems = new List<ConditionalItem>
                {
                    new ConditionalItem(ItemId.Sunfire_Cape),
                    new ConditionalItem(ItemId.Mercurys_Treads,ItemId.Ninja_Tabi,ItemCondition.ENEMY_AP),
                    new ConditionalItem(ItemId.Abyssal_Scepter, ItemId.Iceborn_Gauntlet,ItemCondition.ENEMY_AP),
                    new ConditionalItem(ItemId.Warmogs_Armor),
                    new ConditionalItem(ItemId.Rylais_Crystal_Scepter),
                    new ConditionalItem(ItemId.Banshees_Veil,ItemId.Thornmail,ItemCondition.ENEMY_AP),
                },
                startingItems = new List<ItemId>
                {
                    ItemId.Giants_Belt
                }
            };
        }

        private void InterrupterOnOnPossibleToInterrupt(Obj_AI_Base unit, Interrupter.InterruptableSpellEventArgs spell)
        {
            if (spell.DangerLevel != DangerLevel.High)
            {
                return;
            }


            if (Q.IsReady() & Q.IsInRange(unit))
            {
                Q.Cast(unit);
            }


            if (R.IsReady() && R.IsInRange(unit))
            {
                R.Cast(unit);
            }


        }

        public override void useQ(Obj_AI_Base target)
        {
            var coolstuff = Q.GetPrediction(target);
            //coolstuff.CollisionObjects;
            if (!Q.IsReady())
                return;
            //TODO
            if (safeGap(target))
            {
                /*var hitchance = Q.GetPrediction(target, false, 0,
                    new[]
                    {
                       coolstuff.CollisionObjects
                    }).HitChance;*/

                if (coolstuff.HitChance >= HitChance.Medium)
                {
                    Q.Cast(target);
                }
            }
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
            E.Cast();
        }

        public override void useR(Obj_AI_Base target)
        {
            if (!R.IsReady())
                return;
            R.Cast(target);
        }

        public override void setUpSpells()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 1100, SkillShotType.Linear, (int)0.5f, (int?)1900f, 90);
            {
                AllowedCollisionCount = 0;
            }
            W = new Spell.Active(SpellSlot.W);
            E = new Spell.Active(SpellSlot.E, 300);
            R = new Spell.Targeted(SpellSlot.R, (uint)ObjectManager.Player.Spellbook.GetSpell(SpellSlot.R).SData.CastRange);
            /*Q = new Spell(SpellSlot.Q, 950f);
            Q.SetSkillshot(250, 90f, 2000f, true, SkillshotType.SkillshotLine);
           
            W = new Spell(SpellSlot.W, 400f);
            E = new Spell(SpellSlot.E, 350f);
            R = new Spell(SpellSlot.R, 755f);*/
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