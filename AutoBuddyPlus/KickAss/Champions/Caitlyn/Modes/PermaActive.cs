using System;
using EloBuddy;
using EloBuddy.SDK;

using Settings = KickassSeries.Champions.Caitlyn.Config.Modes.Misc;


namespace KickassSeries.Champions.Caitlyn.Modes
{
    public sealed class PermaActive : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return true;
        }

        private int _lastW;

        public override void Execute()
        {/*
            if (Player.Instance.CanAttack && Settings.UseAA)
            {
                var targetAutoAttack = TargetSelector.GetTarget(Player.Instance.AttackRange*2, DamageType.Physical);
                if (targetAutoAttack != null)
                {
                    if (targetAutoAttack.HasBuff("caitlynyordletrapinternal"))
                    {
                        Player.IssueOrder(GameObjectOrder.AttackUnit, targetAutoAttack);
                    }
                }
            }

            if (Settings.UseQCC && Q.IsReady())
            {
                var target = TargetSelector.GetTarget(Q.Range, DamageType.Physical);

                if (target != null)
                {
                    if (target.HasBuffOfType(BuffType.Stun) || target.HasBuffOfType(BuffType.Snare) ||
                        target.HasBuffOfType(BuffType.Knockup))
                    {
                        Q.Cast(target);
                    }
                }
            }

            if (Settings.UseWCC && _lastW + 1500 > Environment.TickCount && W.IsReady())
            {
                var target = TargetSelector.GetTarget(W.Range, DamageType.Physical);
                if (target != null)
                {
                    if (target.HasBuffOfType(BuffType.Stun) || target.HasBuffOfType(BuffType.Snare) ||
                        target.HasBuffOfType(BuffType.Knockup))
                    {
                        W.Cast(target);
                        _lastW = Environment.TickCount;
                    }
                }
            }

            if (R.IsReady() && Settings.KeyR)
            {
                var targetR = TargetSelector.GetTarget(SpellManager.R.Range, DamageType.Physical);
                if (targetR != null)
                {
                    if (targetR.IsValidTarget(R.Range) &&
                        targetR.Health < SpellDamage.GetRealDamage(SpellSlot.R, targetR))
                    {
                        R.Cast(targetR);
                    }
                }
            }*/
        }
    }
}

