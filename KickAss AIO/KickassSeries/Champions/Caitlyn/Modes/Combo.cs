using System;
using EloBuddy;
using EloBuddy.SDK;

using Settings = KickassSeries.Champions.Caitlyn.Config.Modes.Combo;

namespace KickassSeries.Champions.Caitlyn.Modes
{
    public sealed class Combo : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo);
        }

        private int _lastW;

        public override void Execute()
        {
            var target = TargetSelector.GetTarget(Q.Range, DamageType.Physical);
            if (target == null) return;

            if (Settings.UseE && E.IsReady() && target.IsInRange(Player.Instance, 490) &&
                target.IsValidTarget(E.Range) && target.IsFacing(Player.Instance))
            {
                E.Cast(target);
            }

            if (Settings.UseQ && Q.IsReady() && target.IsValidTarget(Q.Range))
            {
                if (!target.IsInRange(Player.Instance, Player.Instance.AttackRange) ||
                    target.HasBuffOfType(BuffType.Slow))
                    Q.Cast(target);
            }

            if (W.IsReady() && target.IsValidTarget(W.Range) && _lastW + 16000 > Environment.TickCount)
            {
                W.Cast(target);
                _lastW = Environment.TickCount;
            }

            if (!Player.Instance.CanAttack) return;

            var targetAutoAttack = TargetSelector.GetTarget(Player.Instance.AttackRange*2, DamageType.Physical);
            if (targetAutoAttack == null) return;

            if (targetAutoAttack.HasBuff("caitlynyordletrapinternal"))
            {
                Player.IssueOrder(GameObjectOrder.AttackUnit, targetAutoAttack);
            }
        }
    }
}