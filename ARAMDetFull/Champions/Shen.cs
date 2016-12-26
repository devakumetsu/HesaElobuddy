using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using SharpDX;

namespace ARAMDetFull.Champions
{
    class Shen : Champion
    {
        public static Vector2 QCastPos = new Vector2();


        public Shen()
        {
            ARAMSimulator.champBuild = new Build
            {
                coreItems = new List<ConditionalItem>
                {
                    new ConditionalItem(ItemId.Mercurys_Treads,ItemId.Ninja_Tabi,ItemCondition.ENEMY_AP),
                    new ConditionalItem(ItemId.Sunfire_Cape),
                    new ConditionalItem(ItemId.Spirit_Visage),
                    new ConditionalItem(ItemId.Warmogs_Armor),
                    new ConditionalItem(ItemId.Locket_of_the_Iron_Solari,ItemId.Randuins_Omen,ItemCondition.ENEMY_AP),
                    new ConditionalItem(ItemId.Banshees_Veil,ItemId.Thornmail, ItemCondition.ENEMY_AP),
                },
                startingItems = new List<ItemId>
                {
                    ItemId.Bamis_Cinder
                }
            };
            Gapcloser.OnGapcloser += OnEnemyGapcloser;
            Interrupter.OnInterruptableSpell += OnPossibleToInterrupt;
        }

        private void OnPossibleToInterrupt(Obj_AI_Base sender, Interrupter.InterruptableSpellEventArgs args)
        {
            if (player.IsDead || !E.CanCast(sender))
            {
                return;
            }
            var predE = E.GetPrediction(sender, true);
            if (predE.HitChance >= HitChance.Medium)
            {
                E.Cast(predE.CastPosition.Extend(player.ServerPosition, -100).To3D());
            }
        }

        private void OnEnemyGapcloser(AIHeroClient args, Gapcloser.GapcloserEventArgs gapcloser)
        {
            if (player.IsDead || !E.CanCast(gapcloser.Sender))
            {
                return;
            }
            var predE = E.GetPrediction(gapcloser.Sender, true);
            if (predE.HitChance >= HitChance.Low)
            {
                E.Cast(predE.CastPosition.Extend(player.ServerPosition, -100).To3D());
            }
        }

        public override void useQ(Obj_AI_Base target)
        {
            if (!Q.IsReady() || target == null)
                return;
            Q.Cast(target);
        }

        public override void useW(Obj_AI_Base target)
        {
            if (!W.IsReady() || target == null)
                return;
            //if (!Q.IsReady(4500) && player.Mana > 200)
            W.Cast();
        }

        public override void useE(Obj_AI_Base target)
        {
            if (!E.IsReady() || target == null || !safeGap(target))
                return;
            E.Cast(target);
        }

        public override void useR(Obj_AI_Base target)
        {
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
            var obj = EntityManager.Heroes.Allies.Where(i => !i.IsMe && i.IsValidTarget(R.Range, false) && i.HealthPercent < 35 && i.CountEnemiesInRange(E.Range) > 0).OrderBy(i => i.Health).FirstOrDefault();
            if (R.IsReady())
            if (obj != null)
            {
                R.Cast(obj);
                Aggresivity.addAgresiveMove(new AgresiveMove(105, 6000, true));
            }
        }



        public override void setUpSpells()
        {
            Q = new Spell.Active(SpellSlot.Q, 400);
            W = new Spell.Active(SpellSlot.W, 400);
            E = new Spell.Skillshot(SpellSlot.E, 600, SkillShotType.Linear, 500, 1600, 50);
            R = new Spell.Targeted(SpellSlot.R, 35000);

        }


        public override void farm()
        {
            if (player.ManaPercent < 55)
                return;

            var AllMinions = EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy, player.ServerPosition, Q.Range);
            foreach (var minion in AllMinions)
            {
                if (Q.IsReady() && Q.GetDamage(minion) > minion.Health)
                {
                    Q.Cast(minion);
                }
            }
        }
    }
}