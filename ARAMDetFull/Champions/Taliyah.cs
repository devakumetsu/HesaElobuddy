using System;
using System.Collections.Generic;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using SharpDX;

namespace ARAMDetFull.Champions
{
    class Taliyah : Champion
    {
        private static Vector3 lastE;
        private static int lastETick = Environment.TickCount;

        public int AllowedCollisionCount { get; private set; }

        //private static bool Q5x = true;
        private static bool EWCasting = false;
        public Taliyah()
        {
            ARAMSimulator.champBuild = new Build
            {
                coreItems = new List<ConditionalItem>
                {
                    new ConditionalItem(ItemId.Rod_of_Ages),
                    new ConditionalItem(ItemId.Sorcerers_Shoes),
                    new ConditionalItem(ItemId.Athenes_Unholy_Grail),
                    new ConditionalItem(ItemId.Zhonyas_Hourglass),
                    new ConditionalItem(ItemId.Rabadons_Deathcap),
                    new ConditionalItem(ItemId.Void_Staff),
                },
                startingItems = new List<ItemId>
                {
                    ItemId.Catalyst_of_Aeons
                }
            };
            Gapcloser.OnGapcloser += AntiGapcloser_OnEnemyGapcloser;
            Interrupter.OnInterruptableSpell += OnInterruptableSpell;
        }

        public override void useQ(Obj_AI_Base target)
        {
            if (!Q.IsReady())
                return;
            var pred = Q.GetPrediction(target);
            if (pred.HitChance > HitChance.High || player.CountEnemiesInRange(Q.Range) > 1)
                Q.Cast(target);
        }

        public override void useW(Obj_AI_Base target)
        {
            if (!W.IsReady())
                return;
            var pred = W.GetPrediction(target);
            if (pred.HitChance >= HitChance.High)
                W.Cast(pred.UnitPosition);
        }

        public override void useE(Obj_AI_Base target)
        {
            if (!E.IsReady())
                return;

            if (W.IsReady() && W.IsInRange(target))
            {
                var pred = W.GetPrediction(target);
                if (pred.HitChance >= HitChance.High)
                {
                    lastE = ObjectManager.Player.ServerPosition;
                    E.Cast(ObjectManager.Player.ServerPosition +
                           (pred.CastPosition - ObjectManager.Player.ServerPosition).Normalized() * (E.Range - 200));
                    Core.DelayAction(() =>
                    {
                        W.Cast(pred.UnitPosition);
                        EWCasting = false;
                    }, 250);
                    EWCasting = true;
                }
            }
            else
            {
                E.Cast(target);
                lastE = ObjectManager.Player.ServerPosition;
            }
        }

        public override void useR(Obj_AI_Base target)
        {
            if (!R.IsReady())
                return;
            // if (player.CountEnemiesInRange(450) > 1 || player.HealthPercent < 25)
            //     R.Cast();
        }

        public override void killSteal()
        {
            if (!W.IsReady())
                return;
            var killable = EntityManager.Heroes.Enemies.Where(h => h.Health < W.GetDamage(h) && W.IsInRange(h)).ToList();
            if (killable != null && killable.Any())
            {
                var pred = W.GetPrediction(killable.FirstOrDefault());
                if (pred.HitChance >= HitChance.High)
                    W.Cast(pred.UnitPosition);
            }
        }

        public override void setUpSpells()
        {
            #region new skill
            Q = new Spell.Skillshot(SpellSlot.Q, 900, SkillShotType.Linear, 0, 2000, 60);
            {
                AllowedCollisionCount = 0;
            }
            W = new Spell.Skillshot(SpellSlot.W, 800, SkillShotType.Circular, 250, int.MaxValue, 180);
            E = new Spell.Skillshot(SpellSlot.E, 700, SkillShotType.Cone);
            //R = new Spell.Skillshot(SpellSlot.R, 3000, SkillShotType.Linear);
            #endregion
            /*Q = new Spell(SpellSlot.Q, 900f);
            Q.SetSkillshot(0f, 60f, Q.Instance.SData.MissileSpeed, true, SkillshotType.SkillshotLine);

            W = new Spell(SpellSlot.W, 900f);
            W.SetSkillshot(0.5f, 50f, float.MaxValue, false, SkillshotType.SkillshotCircle);

            E = new Spell(SpellSlot.E, 600f);
            E.SetSkillshot(0.25f, 150f, 2000f, false, SkillshotType.SkillshotLine);*/
        }

        public override void useSpells()
        {
            try
            {
                if (player.Spellbook.IsChanneling)
                    return;
                var tar = ARAMTargetSelector.getBestTarget(Q.Range);
                if (tar != null) useQ(tar);
                tar = ARAMTargetSelector.getBestTarget(E.Range);
                if (tar != null) useE(tar);
                tar = ARAMTargetSelector.getBestTarget(W.Range);
                if (tar != null) useW(tar);
                // tar = ARAMTargetSelector.getBestTarget(R.Range);
                //if (tar != null) useR(tar);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

        }

        private void OnInterruptableSpell(Obj_AI_Base unit, Interrupter.InterruptableSpellEventArgs spell)
        {
            if (W.IsReady() && unit.IsValidTarget(W.Range))
                W.Cast(unit.ServerPosition);
        }

        private void AntiGapcloser_OnEnemyGapcloser(AIHeroClient unit, Gapcloser.GapcloserEventArgs gapcloser)
        {
            if (E.IsReady() && gapcloser.Sender.IsValidTarget(W.Range))
                W.Cast(gapcloser.Sender.ServerPosition);
        }

    }
}