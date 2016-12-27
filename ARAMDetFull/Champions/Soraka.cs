using System.Collections.Generic;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;

namespace ARAMDetFull.Champions
{
    class Soraka : Champion
    {
        public Soraka()
        {
            Interrupter.OnInterruptableSpell += InterrupterOnOnPossibleToInterrupt;
            Gapcloser.OnGapcloser += AntiGapcloserOnOnEnemyGapcloser;

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


        private void AntiGapcloserOnOnEnemyGapcloser(AIHeroClient target, Gapcloser.GapcloserEventArgs gapcloser)
        {
            var unit = gapcloser.Sender;

            if (unit.IsValidTarget(Q.Range) && Q.IsReady())
            {
                Q.Cast(target);
            }
            if (unit.IsValidTarget(E.Range) && E.IsReady())
            {
                E.Cast(target);
            }
        }

        private void InterrupterOnOnPossibleToInterrupt(Obj_AI_Base sender, Interrupter.InterruptableSpellEventArgs args)
        {
            var unit = sender;
            var spell = args;
            
            if (!unit.IsValidTarget(E.Range))
            {
                return;
            }
            if (!E.IsReady())
            {
                return;
            }

            E.Cast(unit);
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

        }

        public override void useE(Obj_AI_Base target)
        {
            if (E.IsReady())
                E.Cast(target);
        }


        public override void useR(Obj_AI_Base target)
        {
        }

        public override void useSpells()
        {
            var tar = ARAMTargetSelector.getBestTarget(Q.Range);
            if (tar != null) useQ(tar);
            tar = ARAMTargetSelector.getBestTarget(E.Range);
            if (tar != null) useE(tar);

        }

        public override void farm()
        {
            AutoR();
            AutoW();
            if (player.ManaPercent < 55 || !Q.IsReady())
                return;

            foreach (var minion in EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy, player.Position, Q.Range))
            {
                if (minion.Health < Q.GetDamage(minion))
                {
                    Q.Cast(minion);
                    return;
                }
            }
        }

        public override void killSteal()
        {
            base.killSteal();
            AutoR();
            AutoW();
        }

        public override void setUpSpells()
        {
            //Create the spells
            Q = new Spell.Skillshot(SpellSlot.Q, 800, SkillShotType.Circular, 283, 1100, 210)
            {
                AllowedCollisionCount = int.MaxValue
            };
            W = new Spell.Targeted(SpellSlot.W, 550);
            E = new Spell.Skillshot(SpellSlot.E, 925, SkillShotType.Circular, 500, 1750, 70)
            {
                AllowedCollisionCount = int.MaxValue
            };
            R = new Spell.Active(SpellSlot.R, 25000);
        }

        private void AutoR()
        {
            if (!R.IsReady())
            {
                return;
            }

            foreach (var friend in
               EntityManager.Heroes.Allies.Where(x => x.IsAlly).Where(x => !x.IsDead).Where(x => !x.IsZombie))
            {
                var health = 35;

                if (friend.HealthPercent <= health)
                {
                    R.Cast();
                }
            }
        }

        private void AutoW()
        {
            if (!W.IsReady())
            {
                return;
            }
            if (player.HealthPercent < 45)
                return;
            foreach (var friend in
                from friend in
                    EntityManager.Heroes.Allies
                        .Where(x => !x.IsEnemy && !x.IsMe)
                        .Where(friend => W.IsInRange(friend.Position))
                let healthPercent = 75
                where friend.HealthPercent <= healthPercent
                select friend)
            {
                W.CastOnUnit(friend);
            }
        }
    }
}