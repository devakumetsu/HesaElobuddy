using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoBuddy.MainLogics;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;

namespace AutoBuddy.MyChampLogic
{
    internal class Aatrox : IChampLogic
    {
        public float MaxDistanceForAA { get { return int.MaxValue; } }
        public float OptimalMaxComboDistance { get { return AutoWalker.myHero.AttackRange; } }
        public float HarassDistance { get { return AutoWalker.myHero.AttackRange; } }

        public Spell.Skillshot Q, E;
        public Spell.Active W, R;

        public int[] skillSequence { get; private set; }
        public LogicSelector Logic { get; set; }

        public Aatrox()
        {
            skillSequence = new[] { 2, 3, 1, 3, 3, 4, 3, 2, 3, 2, 4, 2, 2, 1, 1, 4, 1, 1 };

            this.Q = new Spell.Skillshot(SpellSlot.Q, 650, SkillShotType.Circular, 250, 450, 285) { AllowedCollisionCount = int.MaxValue };
            this.W = new Spell.Active(SpellSlot.W);
            this.E = new Spell.Skillshot(SpellSlot.E, 1000, SkillShotType.Linear, 250, 1200, 100) { AllowedCollisionCount = int.MaxValue };
            this.R = new Spell.Active(SpellSlot.R, 500);
            
            Interrupter.OnInterruptableSpell += Interrupter_OnInterruptableSpell;
            Gapcloser.OnGapcloser += Gapcloser_OnGapcloser;
        }
        
        private void Gapcloser_OnGapcloser(AIHeroClient sender, Gapcloser.GapcloserEventArgs e)
        {
            if (sender == null || !sender.IsEnemy || !sender.IsKillable(Q.Range) || !Q.IsReady() || Player.Instance.PredictHealthPercent() < 15)
                return;
            Q.Cast(sender);
        }

        private void Interrupter_OnInterruptableSpell(Obj_AI_Base sender, Interrupter.InterruptableSpellEventArgs e)
        {
            if (sender == null || !sender.IsEnemy || !sender.IsKillable(Q.Range) || !Q.IsReady() || Player.Instance.PredictHealthPercent() < 15)
                return;
            Q.Cast(sender);
        }

        public void Harass(AIHeroClient target)
        {
            if (target == null || !target.IsKillable(Q.Range)) return;

            if (Q.IsReady() && Player.Instance.PredictHealthPercent() > 25)
            {
                QAOE(target);
            }
            if (W.IsReady())
            {
                if (W.Handle.ToggleState == 1 && Player.Instance.PredictHealthPercent() > 50)
                {
                    W.Cast();
                }
                else
                {
                    W.Cast();
                }
            }
            if (E.IsReady() && target.IsKillable(E.Range))
            {
                E.Cast(target, HitChance.Medium);
            }
        }

        public void Survi()
        {
            var target = TargetSelector.GetTarget(E.Range, DamageType.Physical);
            if (target == null) return;
            if (E.IsReady())
            {
                E.Cast(target);
            }
        }

        public void Combo(AIHeroClient target)
        {
            if (target == null || !target.IsKillable(Q.Range)) return;

            if (Q.IsReady())
            {
                QAOE(target);
            }
            if (W.IsReady())
            {
                if (W.Handle.ToggleState == 1 && Player.Instance.PredictHealthPercent() > 50)
                {
                    W.Cast();
                }
                else
                {
                    W.Cast();
                }
            }
            if (E.IsReady() && target.IsKillable(E.Range))
            {
                E.Cast(target, HitChance.Medium);
            }
            if (R.IsReady())
            {
                RAOE();
            }
        }

        private void QAOE(Obj_AI_Base target)
        {
            if (Q.GetPrediction(target).CastPosition.CountEnemyHeros(((Spell.Skillshot)Q).Width) >= 2)
            {
                var pos = Q.GetPrediction(target).CastPosition;
                Q.Cast(target, HitChance.Medium);
            }
        }

        private void RAOE()
        {
            if (Player.Instance.CountEnemyHeros((int)R.Range) >= 2)
            {
                R.Cast();
            }
        }
        
    }
}