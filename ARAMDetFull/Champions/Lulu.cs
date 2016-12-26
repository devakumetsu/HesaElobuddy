using System;
using System.Collections.Generic;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;

namespace ARAMDetFull.Champions
{
    class Lulu : Champion
    {
        private GameObject pix = player;
        private Spell.Skillshot Q2;

        public int AllowedCollisionCount { get; private set; }

        public Lulu()
        {
            Interrupter.OnInterruptableSpell += Interrupter2_OnInterruptableTarget;
            Gapcloser.OnGapcloser += AntiGapcloser_OnEnemyGapcloser;

            GameObject.OnCreate += onCreate;
            GameObject.OnDelete += onDelete;

            ARAMSimulator.champBuild = new Build
            {
                coreItems = new List<ConditionalItem>
                {
                    new ConditionalItem(ItemId.Athenes_Unholy_Grail),
                    new ConditionalItem(ItemId.Ionian_Boots_of_Lucidity),
                    new ConditionalItem(ItemId.Rabadons_Deathcap),
                    new ConditionalItem(ItemId.Ludens_Echo, ItemId.Abyssal_Scepter,ItemCondition.ENEMY_LOSING),
                    new ConditionalItem(ItemId.Lich_Bane),
                    new ConditionalItem(ItemId.Banshees_Veil,ItemId.Zhonyas_Hourglass,ItemCondition.ENEMY_AP),
                },
                startingItems = new List<ItemId>
                {
                    ItemId.Fiendish_Codex
                }
            };
        }

        private void onDelete(GameObject sender, EventArgs args)
        {
            if (pix != null && sender.NetworkId == pix.NetworkId)
                pix = player;
        }

        private void onCreate(GameObject sender, EventArgs args)
        {
            if (sender.IsAlly && sender.Name == "RobotBuddy")
                pix = sender;
        }

        private void AntiGapcloser_OnEnemyGapcloser(Obj_AI_Base sender, Gapcloser.GapcloserEventArgs args)
        {
            // use W against gap closer
            var target = sender;
            if (W.IsReady() && target.IsValidTarget(W.Range))
            {
                W.Cast(target);
            }
        }

        private void Interrupter2_OnInterruptableTarget(Obj_AI_Base sender, Interrupter.InterruptableSpellEventArgs args)
        {
            // interrupt with W
            if (W.IsReady() && sender.IsValidTarget(W.Range) && !sender.IsZombie)
            {
                W.Cast(sender);
            }
            // interrupt with R
            else if (R.IsReady() && sender.IsValidTarget() && !sender.IsZombie)
            {
                var target = EntityManager.Heroes.Allies.Where(x => x.IsValidTarget(R.Range)).OrderByDescending(x => 1 - x.Distance(sender.Position)).FirstOrDefault(x => x.Distance(sender.Position) <= 350);
                if (target != null)
                    R.Cast(target);
            }
        }

        public override void useQ(Obj_AI_Base target)
        {
            if (Q.IsReady() && target.IsValid)
                Q.Cast(target);
        }

        public override void useW(Obj_AI_Base target)
        {
            if (!W.IsReady())
                return;
            W.CastOnUnit(target);
        }

        public override void useE(Obj_AI_Base target)
        {
            if (!E.IsReady())
                return;
            E.Cast(target);
        }

        public override void useR(Obj_AI_Base target)
        {
            if (!R.IsReady())
                return;
            foreach (var hero in EntityManager.Heroes.Allies.Where(x => x.IsValidTarget(R.Range)))
            {
                if (hero.CountEnemiesInRange(350) >= 1)
                    R.Cast(hero);
            }
        }

        public override void setUpSpells()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 950, SkillShotType.Linear, 250, 1450, 60);
            {
                AllowedCollisionCount = int.MaxValue;
            }
            Q2 = new Spell.Skillshot(SpellSlot.Q, 1600, SkillShotType.Linear, 250, 1450, 60);
            {
                AllowedCollisionCount = int.MaxValue;
            }

            W = new Spell.Targeted(SpellSlot.W, 650);
            E = new Spell.Targeted(SpellSlot.E, 650);
            R = new Spell.Targeted(SpellSlot.R, 900);
        }

        public override void useSpells()
        {
            var tar = ARAMTargetSelector.getBestTarget(Q.Range, true, pix.Position);
            if (tar != null) useQ(tar);
            tar = ARAMTargetSelector.getBestTarget(W.Range);
            if (tar != null) useW(tar);
            tar = ARAMTargetSelector.getBestTarget(E.Range);
            if (tar != null) useE(tar);
            tar = ARAMTargetSelector.getBestTarget(R.Range);
            if (tar != null) useR(tar);
        }

        public override void killSteal()
        {
            base.killSteal();
            // case KS with Q
            if (Q.IsReady())
            {
                foreach (var hero in EntityManager.Heroes.Enemies.Where(x => x.IsValidTarget() && Q.GetDamage(x) >= x.Health
                    && (x.Distance(player.Position) > x.Distance(pix.Position) ? 925 >= x.Distance(pix.Position) : 925 >= x.Distance(player.Position))
                    ))
                {
                    Q.Cast(hero);
                    //TODO fix Q2
                    //Q2.SetSkillshot(0.25f, 70, 1450, false, SkillshotType.SkillshotLine, pix.Position, pix.Position);
                    Q2.Cast(hero);
                }
            }
            // case KS with E
            if (E.IsReady())
            {
                foreach (var hero in EntityManager.Heroes.Enemies.Where(x => x.IsValidTarget(E.Range) && E.GetDamage(x) >= x.Health))
                {
                    E.Cast(hero);
                }
            }


            // case KS with EQ
            if (Q.IsReady() && E.IsReady() && player.Mana >= Q.ManaCost + E.ManaCost)
            {
                // EQ on same target
                foreach (var hero in EntityManager.Heroes.Enemies.Where(x => x.IsValidTarget(E.Range) && E.GetDamage(x) + Q.GetDamage(x) >= x.Health
                    && Q.GetDamage(x) < x.Health))
                {
                    E.Cast(hero);
                }
                // EQ on different target
                foreach (var hero in EntityManager.Heroes.Enemies.Where(x => x.IsValidTarget(E.Range + Q.Range) && !x.IsValidTarget(Q.Range)
                    && Q.GetDamage(x) >= x.Health))
                {
                    // E target is hero
                    foreach (var target in EntityManager.Heroes.AllHeroes.Where(x => x.IsValidTarget(E.Range, false) && x.Distance(hero.Position) <= Q.Range)
                        .OrderByDescending(y => 1 - y.Distance(hero.Position)))
                    {
                        E.Cast(target);
                    }
                    // E target is minion
                    foreach (var target in EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy, ObjectManager.Player.Position, E.Range).Where(x => x.IsValidTarget(E.Range, false)
                          && !x.Name.ToLower().Contains("ward") && x.Distance(hero.Position) <= Q.Range)
                            .OrderByDescending(y => 1 - y.Distance(hero.Position)))
                    {
                        // target die with E ?
                        if (!target.IsAlly && target.Health > E.GetDamage(target) || target.IsAlly)
                            E.Cast(target);
                    }
                }
            }
            //auto shield
            if (E.IsReady())
            {
                foreach (var hero in EntityManager.Heroes.Allies.Where(x => x.IsValidTarget(E.Range, false)))
                {
                    if (hero.Health * 100 / hero.MaxHealth <= 36
                        && hero.CountEnemiesInRange(900) >= 1)
                        E.Cast(hero);
                }
            }
            //auto R save
            if (R.IsReady())
            {
                foreach (var hero in EntityManager.Heroes.Allies.Where(x => x.IsValidTarget(R.Range, false)))
                {
                    if (hero.Health * 100 / hero.MaxHealth <= 20
                        && hero.CountEnemiesInRange(500) >= 1)
                        R.Cast(hero);
                }
            }
        }
    }
}