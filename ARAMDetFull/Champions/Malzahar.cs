using System.Collections.Generic;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;

namespace ARAMDetFull.Champions
{
    class Malzahar : Champion
    {

        public Malzahar()
        {
            Interrupter.OnInterruptableSpell += InterrupterOnOnPossibleToInterrupt;
            Gapcloser.OnGapcloser += AntiGapcloserOnOnEnemyGapcloser;

            ARAMSimulator.champBuild = new Build
            {
                coreItems = new List<ConditionalItem>
                {
                    new ConditionalItem(ItemId.Rod_of_Ages),
                    new ConditionalItem(ItemId.Sorcerers_Shoes),
                    new ConditionalItem(ItemId.Rylais_Crystal_Scepter),
                    new ConditionalItem(ItemId.Liandrys_Torment),
                    new ConditionalItem(ItemId.Abyssal_Scepter, ItemId.Zhonyas_Hourglass, ItemCondition.ENEMY_AP),
                    new ConditionalItem(ItemId.Rabadons_Deathcap),
                },
                startingItems = new List<ItemId>
                {
                    ItemId.Catalyst_of_Aeons,ItemId.Boots_of_Speed
                }
            };
        }

        private void InterrupterOnOnPossibleToInterrupt(Obj_AI_Base unit, Interrupter.InterruptableSpellEventArgs args)
        {
            if (!unit.IsValidTarget())
            {
                return;
            }

            if (args.DangerLevel != DangerLevel.High)
            {
                return;
            }
            Q.Cast(unit);
        }
        //TODO not sure if sender will work here
        private void AntiGapcloserOnOnEnemyGapcloser(Obj_AI_Base sender, Gapcloser.GapcloserEventArgs args)
        {
            if (!sender.IsValidTarget())
            {
                return;
            }
            Q.Cast(sender);
        }

        public override void useQ(Obj_AI_Base target)
        {
            if (!Q.IsReady() || target == null)
                return;
            Q.Cast(target);
        }

        public override void useW(Obj_AI_Base target)
        {
            if (!W.IsReady())
                return;
            if (target.IsValidTarget(W.Range))
            {
                W.Cast();
            }
        }

        public override void useE(Obj_AI_Base target)
        {
            if (!E.IsReady() || target == null)
                return;
            E.CastOnUnit(target);
        }


        public override void useR(Obj_AI_Base target)
        {
            if (!R.IsReady() || target == null)
                return;
            R.Cast(target);
        }

        public override void useSpells()
        {
            if (ObjectManager.Player.Spellbook.IsChanneling)
                return;
            var tar = ARAMTargetSelector.getBestTarget(Q.Range);
            if (tar != null) useQ(tar);
            tar = ARAMTargetSelector.getBestTarget(W.Range);
            if (tar != null) useW(tar);
            tar = ARAMTargetSelector.getBestTarget(E.Range);
            if (tar != null) useE(tar);
            tar = ARAMTargetSelector.getBestTarget(R.Range);
            if (tar != null) useR(tar);
        }

        public override void setUpSpells()
        {
            //Create the spells
            Q = new Spell.Skillshot(SpellSlot.Q, 900, SkillShotType.Linear, 1000, int.MaxValue, 85, DamageType.Magical)
            { AllowedCollisionCount = -1 };
            W = new Spell.Skillshot(SpellSlot.W, 450, SkillShotType.Circular, 250, int.MaxValue, 250)
            { AllowedCollisionCount = -1 };
            E = new Spell.Targeted(SpellSlot.E, 650, DamageType.Magical);
            R = new Spell.Targeted(SpellSlot.R, 700, DamageType.Magical);

            //Q.SetSkillshot(0.5f, 100, float.MaxValue, false, SkillshotType.SkillshotLine);
            //W.SetSkillshot(0.5f, 240, 20, false, SkillshotType.SkillshotCircle);
        }

        public override void farm()
        {
            if (player.ManaPercent < 40)
                return;
            if (E.IsReady())
            {
                var mins = EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy, Player.Instance.Position, E.Range).Where(min => min.Health < E.GetDamage(min)).ToList();
                if (mins.Count() != 0)
                    E.CastOnUnit(mins.FirstOrDefault());
            }

        }
    }
}